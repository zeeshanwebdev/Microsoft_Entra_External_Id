using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{

    public class AccountController : Controller
    {
        public new IActionResult SignOut()
        {
            base.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
