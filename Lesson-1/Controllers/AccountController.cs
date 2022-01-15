using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (ModelState.IsValid == false)
            return View(model);

        var user = new User()
        {
            UserName = model.UserName,
            // PasswordHash = model.Password.GetHashCode().ToString()
        };

        var registerResult = await _userManager.CreateAsync(user, model.Password);
        if (!registerResult.Succeeded)
        {
            foreach (var error in registerResult.Errors)
            {
                ModelState.AddModelError(error.Code,error.Description);
            }

            return View(model);
        }

        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login(string returnUrl) => View(new LoginUserViewModel(){ReturnUrl = returnUrl});

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserViewModel model)
    {
        var signInResult = await _signInManager.PasswordSignInAsync
            (
                model.UserName,
                model.Password,
                model.RememberMe,
                true
            );
        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("","UserName or Password is incorrect");
            return View(model);
        }

        return LocalRedirect(model.ReturnUrl ?? "/");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();

}