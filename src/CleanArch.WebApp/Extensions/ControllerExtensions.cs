using Microsoft.AspNetCore.Mvc;

namespace CleanArch.WebApp.Extensions
{
    public static class ControllerExtensions
    {
        public static void SuccessNotify(this Controller controller, string message)
        {
            controller.TempData["SuccessMessage"] = message;
        }

        public static void ErrorNotify(this Controller controller, string message)
        {
            controller.TempData["ErrorMessage"] = message;
        }
    }
}
