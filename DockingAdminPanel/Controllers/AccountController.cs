using DockingAdminPanel.Data;
using DockingAdminPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.Metrics;
using System.Text.Encodings.Web;
using System.Text;
using static DockingAdminPanel.Areas.Identity.Pages.Account.LoginModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;

namespace DockingAdminPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        ILogger<AccountController> _logger;
        private readonly IUserStore<User> _userStore;
        private readonly IEmailSender _emailSender;
        private readonly BookingWebAppContext _context;
        //private readonly IUserEmailStore<User> _emailStore;
        public AccountController(ILogger<AccountController> logger,
                                 SignInManager<User> signInManager,
                                 UserManager<User> userManager,
                                 IUserStore<User> userStore,
                                 IEmailSender emailSenders,
                                 BookingWebAppContext context
                                )
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _context = context;
             
            
        }
        [BindProperty]
        public InputModel? Input { get; set; }
        // GET: AccountController
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //Google Recaptcha
         


            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                    model.Password,
                    model.RememberMe,
                    true);

                if (result.Succeeded)
                {
                    //// Update Last Login Column
                    var user = await _userManager.FindByNameAsync(model.Email);

                    if (user == null)
                    {
                        return NotFound("Unable to load user for update last login.");
                    }

                   // user.LastLoginDate = DateTimeOffset.UtcNow;
                    var lastLoginResult = await _userManager.UpdateAsync(user);

                    if (!lastLoginResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Unexpected error occurred setting the last login date" +
                            $" ({lastLoginResult.ToString()}) for user with ID '{user.Id}'.");
                    }

                    ////

                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    //else
                    //{
                    //    var Users = await _userManager.FindByIdAsync(user.Id);
                    //    var Roles = await _userManager.GetRolesAsync(Users);

                    //    if (Roles.Any(x => x == "System Admin"))
                    //    {
                    //        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    //    }
                    //    else if (Roles.Any(x => x == "Trainer"))
                    //    {
                    //        return RedirectToAction("Index", "Dashboard", new { area = "Trainer" });
                    //    }
                    //    else if (Roles.Any(x => x == "Employer"))
                    //    {
                    //        return RedirectToAction("Index", "Dashboard", new { area = "Employer" });
                    //    }
                    //    else if (Roles.Any(x => x == "Agent"))
                    //    {
                    //        return RedirectToAction("Index", "Dashboard", new { area = "Agent" });
                    //    }
                    //    else if (Roles.Any(x => x == "Learner"))
                    //    {
                    //        return RedirectToAction("Dashboard", "Home");
                    //    }
                    //    else if (Roles.Any(x => x == "Stakeholder"))
                    //    {
                    //        return RedirectToAction("Index", "Dashboard", new { area = "Patron" });
                    //    }
                    //    else
                    //    {
                    //        return RedirectToAction("Logout", "Account");
                    //    }
                    //}
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (result.IsLockedOut == true)
                    {
                        ModelState.AddModelError("", "You’ve reached the maximum login attempts. Please try again in 2 minutes.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Username and/or password are incorrect. Please try again.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Failed to login");
            }

            return View();
        }
        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int x)
        {
            //Register a Learner User
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
              //  await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = "" },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = "" });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("Register");
        }
        // GET: AccountController/Create
        public async Task<IActionResult> AllUsers()
        {
            var xx = _userManager.Users;
            var x = await _context.Users.ToListAsync();
            List<UserViewModel> result = new List<UserViewModel>();
            
             foreach (var item in x)
            {
                UserViewModel user = new UserViewModel();
                user.FirstName = item.FirstName;
                user.LastName = item.LastName;
                user.Email = item.Email;
                user.Gender = item.Gender;
                user.Address = item.Address;
                result.Add(user);
               

            }
            return result != null ?
                        View(result) :
                        Problem("Entity set 'BookingWebAppContext.products'  is null.");
        }
        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
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

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
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

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
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
