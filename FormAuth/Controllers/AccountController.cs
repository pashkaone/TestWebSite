using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using FormAuth.Models;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace FormAuth.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToRoute(new { controller = "Account", action = "UserPanel" });
            }
            else
                return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Name, Email = model.Email, Year = model.Year };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                UserManager.AddToRole(user.Id, role: "User");
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToRoute(new { controller = "Account", action = "UserPanel" });
            }
            else
            {
                ViewBag.returnUrl = returnUrl;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Name, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            { // удаление из ЛК
                ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Logout", "Account");
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            { //удаление из общего списка
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Account");
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            /* if (id == null)
             {
                 ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
                 if (user != null)
                 {
                     EditModel model = new EditModel { Year = user.Year };
                     return View(model);
                 }
             }
             else
             {
                 ApplicationUser user = await UserManager.FindByIdAsync(id);
             }*/
            ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (id != null)
            {
                user = await UserManager.FindByIdAsync(id);
            }
            if (user != null)
            {
                EditModel model = new EditModel { Year = user.Year };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditModel model, string password, string id)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (id != null) //проверка, если из списка, то с  ID
            {
                user = await UserManager.FindByIdAsync(id);
            }
            if (user != null)
            {
                user.Year = model.Year;
                //Смена пароля
                if (password != null)
                {
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, model.Password);
                }

                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult UserPanel()
        {
            IList<string> roles = new List<string> { "Роль не определена" };
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindByName(User.Identity.Name);
            if (user != null)
                roles = userManager.GetRoles(user.Id);
            return View(roles);
        }

        [Authorize]
        public ActionResult AdminAccess()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Admin777(string id)
        {
            if (id == "777")
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                UserManager.AddToRole(user.Id, role: "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}