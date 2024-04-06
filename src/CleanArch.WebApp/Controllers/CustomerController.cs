using CleanArch.Application.Enums;
using CleanArch.WebApp.Extensions;
using CleanArch.WebApp.Filters;
using CleanArch.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.WebApp.Controllers
{
    [Authorize(_roles: nameof(Roles.Basic))]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        // GET: CustomerController
        [HttpGet("customers")]
        public async Task<ActionResult> List()
        {
            try
            {
                var response = await _customerService.GetCustomers(new());
                if (response.Succeeded)
                    return View(response.Data);
            }
            catch (Refit.ApiException ex)
            {
                var error = await ex.ErrorResult();
                _logger.LogError(error);
                this.ErrorNotify(error);
            }
            return View();
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
