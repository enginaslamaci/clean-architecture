namespace CleanArch.Application.DTOs.Account.Request;

public class ChangeUserPasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}