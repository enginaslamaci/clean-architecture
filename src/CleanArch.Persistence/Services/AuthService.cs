using CleanArch.Application.Abstractions.Infrastructure.Services;
using CleanArch.Application.Abstractions.Infrastructure.Services.Token;
using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Application.Enums;
using CleanArch.Application.Exceptions;
using CleanArch.Application.Helpers;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace CleanArch.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.ApplicationUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DbSet<RefreshToken> _refreshToken;
        readonly ITokenHandler _tokenHandler;
        readonly IMailService _mailService;
        public AuthService(IConfiguration configuration,
            UserManager<Domain.Entities.Identity.ApplicationUser> userManager,
            ITokenHandler tokenHandler,
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService,
            RoleManager<ApplicationRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _mailService = mailService;
            _roleManager = roleManager;
        }


        #region auth

        public async Task<Result<string>> RegisterAsync(RegistrationRequest request, string origin)
        {
            //if (!(request.Confirmations.Check1 && request.Confirmations.Check2))
            //return await Result<string>.FailAsync("Please Accept The Terms Conditions And Privacy Policy");

            //var userWithSameUserName = await _userManager.FindByNameAsync(request.Email);
            //if (userWithSameUserName is { PhoneNumberConfirmed: false, EmailConfirmed: false })
            //{
            //    var result = await DeleteUserAccount(request.Email);
            //    if (result)
            //    {
            //        return await RegisterAsync(request, origin);
            //    }
            //}

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                IdentityResult result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    var verificationUri = await SendVerificationEmail(user, origin);

                    return await Result<string>.SuccessAsync(user.Id, $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                    throw new ApiException(string.Join(",", result.Errors.Select(x => $"{x.Description} Error:{x.Code}").ToList()));
            }
            throw new BadRequestException("User Email already exists");
        }
        public async Task<Result<string>> ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null)
                throw new BadRequestException($"{model.Email} e-mail address is not registered.");

            //if (string.IsNullOrEmpty(origin)) origin = model.Origin;
            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "reset-password";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var resetUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "token", code);
            resetUri = QueryHelpers.AddQueryString(resetUri, "email", model.Email);

            await _mailService.SendAsync(new Application.DTOs.Email.EmailRequest()
            {
                Subject = "Reset Password",
                To = new List<string>() { model.Email },
                Body = $"<p>You reset token is - {code}</p><p>Use this link:{resetUri}</p>",
            });

            return await Result<string>.SuccessAsync(model.Email, "Email sent");
        }
        public async Task<Result<string>> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) await Result<string>.FailAsync("User not found");
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                await _mailService.SendAsync(new Application.DTOs.Email.EmailRequest()
                {
                    Subject = "Reset Password",
                    To = new List<string>() { user.Email },
                    Body = $"<p>You have successfully protected your password.</p>",
                });

                return await Result<string>.SuccessAsync(model.Email);
            }
            throw new ApiException(result?.Errors?.FirstOrDefault().Description ?? "Error occurred while reset the password!");
        }
        public async Task<Result<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = CustomEncoders.UrlDecode(code); //Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(user.Id, message: $"Account confirmed for {user.Email}. You can now use the /api/v1/account/authenticate endpoint.");
            }
            throw new ApiException($"An error occurred while confirming {user.Email}.");
        }
        public async Task<Result<int>> SignOutAsync(ClaimsPrincipal principal, SignOutRequest request, string ip)
        {

            #region Get User

            // Get the current user
            var user = await _userManager.GetUserAsync(principal);

            // If we have no user...
            if (user is not { Status: StatusType.Active })
                throw new BadRequestException("User not avaliable");

            #endregion

            await _signInManager.SignOutAsync();

            //refresh token revoke
            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                await RevokeToken(request.RefreshToken, ip);
            }

            return await Result<int>.SuccessAsync();
        }
        public async Task<Result<bool>> RevokeToken(string token, string ipAddress)
        {
            var user = _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null)
                throw new NotFoundException();

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive)
                throw new BadRequestException("Refresh token expired");

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            _refreshToken.Update(refreshToken);
            await _userManager.UpdateAsync(user);

            return await Result<bool>.SuccessAsync(true);
        }

        #endregion

        #region internal

        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            Domain.Entities.Identity.ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new BadRequestException("username or password is incorrect");

            if (user.Status == StatusType.Deleted || user.Status == StatusType.Passive || !user.EmailConfirmed)
                throw new BadRequestException("user not avaliable");


            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                var response = await CreateAuthenticationResponse(user);
                return new Result<AuthenticationResponse>() { Succeeded = true, Data = response };
            }
            else
                throw new BadRequestException("username or password is incorrect");
        }
        public async Task<Result<AuthenticationResponse>> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is not { Status: StatusType.Active })
                throw new BadRequestException("User not avaliable");

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive)
                throw new BadRequestException("Refresh token is expired");

            var jwtToken = await _tokenHandler.CreateAccessToken(user);
            var response = new AuthenticationResponse
            {
                Id = user.Id,
                JWToken = jwtToken,
                Email = user.Email,
                UserName = user.FirstName
            };
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            // replace old refresh token with a new one and save
            var newRefreshToken = await _tokenHandler.CreateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            response.RefreshToken = newRefreshToken.Token;

            return await Result<AuthenticationResponse>.SuccessAsync(response, $"Authenticated {user.UserName}");
        }

        #endregion

        #region external

        public async Task<Result<AuthenticationResponse>> ExternalAuthenticate(ExternalAuthDto request)
        {
            // Sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(request.LoginProvider,
               request.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            var email = request.Claims?.Email;

            // If the user already has a login (if there is a record in AspNetUserLogins table)
            if (signInResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var response = await CreateAuthenticationResponse(user);
                return new Result<AuthenticationResponse>() { Succeeded = true, Data = response };
            }
            // If there is no record in AspNetUserLogins table, the user may not have a local account
            else
            {
                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new ApplicationUser()
                        {
                            FirstName = request.Claims.Name,
                            LastName = request.Claims.Surname,
                            UserName = request.Claims.Email,
                            Email = request.Claims.Email,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true
                        };

                        await _userManager.CreateAsync(user);
                        await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    var info = new UserLoginInfo(request.LoginProvider, request.ProviderKey, request.LoginProvider);
                    await _userManager.AddLoginAsync(user, info);

                    var response = await CreateAuthenticationResponse(user);
                    return new Result<AuthenticationResponse>() { Succeeded = true, Data = response };
                }

                // If we cannot find the user email we cannot continue
                throw new BadRequestException($"Email claim not received from: {request.LoginProvider}.");
            }
        }

        //validate google id_token
        public async Task<Result<AuthenticationResponse>> ValidateGoogleToken(GoogleLoginDto model)
        {
            Payload payload = new();
            payload = await ValidateAsync(model.IdToken, new ValidationSettings
            {
                Audience = new[] { "GoogleClientId" }
            });

            var userClaims = new UserClaimsDto
            {
                Name = payload.GivenName,
                Surname = payload.FamilyName,
                Email = payload.Email,
            };

            return await ExternalAuthenticate(new ExternalAuthDto() { LoginProvider = model.Provider, ProviderKey = payload.Subject, Claims = userClaims  });
        }

        #endregion

        #region Private Helpers
        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = CustomEncoders.UrlEncode(code);// WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/v1/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

            //Email Service Call Here
            //await _mailService.SendAsync(new Application.DTOs.Email.EmailRequest()
            //{
            //    Subject = "Register - Email Verification",
            //    To = new List<string>() { user.Email },
            //    Body = $"User Registered. Please confirm your account by visiting this URL {verificationUri}",
            //});

            return verificationUri;
        }

        private async Task<AuthenticationResponse> CreateAuthenticationResponse(ApplicationUser user)
        {
            string jwtToken = await _tokenHandler.CreateAccessToken(user);
            AuthenticationResponse response = new()
            {
                Id = user.Id,
                JWToken = jwtToken,
                Email = user.Email,
                UserName = user.FirstName
            };
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            var refreshToken = await _tokenHandler.CreateRefreshToken("");
            response.RefreshToken = refreshToken.Token;

            user = await _userManager.Users.Include(x => x.RefreshTokens).FirstAsync(x => x.Id == user.Id);
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return response;
        }



        #endregion

    }
}