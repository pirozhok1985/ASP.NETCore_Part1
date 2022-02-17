using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        _logger.LogInformation("Начало процедуры регистрации нового пользователя {0}", model.UserName);
        using (var loggerScope = _logger.BeginScope("Регистрация {0}", model.UserName))
        {
            if (ModelState.IsValid == false)
                return View(model);

            var user = new User()
            {
                UserName = model.UserName,
                // PasswordHash = model.Password.GetHashCode().ToString()
            };

            var registerResult = await _userManager.CreateAsync(user, model.Password);
            _logger.LogInformation("Попытка создать нового пользователя {0}", user.UserName);
            if (!registerResult.Succeeded)
            {
                foreach (var error in registerResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                _logger.LogError("При регистрации пользователя {0} возникли ошибки {1}",
                    user.UserName, String.Join(",", registerResult.Errors.Select(e => e.Description)));
                return View(model);
            }

            _logger.LogInformation("Пользователь {0} успешно зарегистрировался", user.UserName);
            await _userManager.AddToRoleAsync(user, Role.Users);
            await _signInManager.SignInAsync(user, false);
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login(string returnUrl) => View(new LoginUserViewModel() {ReturnUrl = returnUrl});

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserViewModel model)
    {
        _logger.LogInformation("Начало процедуры входа пользователя {0} в систему", model.UserName);
        using (var loggerScope = _logger.BeginScope("Вход пользователя {0}", model.UserName))
        {
            _logger.LogInformation("Попытка входа пользователя {0} в систему", model.UserName);
            var signInResult = await _signInManager.PasswordSignInAsync
            (
                model.UserName,
                model.Password,
                model.RememberMe,
                true
            );
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect");
                _logger.LogError("При входе пользователя {0} возникли ошибки {1}",
                    model.UserName, ModelState.Values
                        .Select(v => String.Join(",", v.Errors
                            .Select(e => e.ErrorMessage))));
                return View(model);
            }

            _logger.LogInformation("Пользователь {0} вошёл в систему", model.UserName);
        }

        return LocalRedirect(model.ReturnUrl ?? "/");
    }

    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Начало процедуры выхода пользователя {0} из систему", User.Identity!.Name);
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Пользователь {0} вышел из системы", User.Identity.Name);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        _logger.LogCritical("Доступ к ресурсу {0} для пользователя {1} запрещён!",
            ControllerContext.HttpContext.Request.Path, User.Identity.Name);
        return View();
    }

    public async Task<IActionResult> ValidateName(string userName)
    {
        var result = await _userManager.FindByNameAsync(userName);
        _logger.LogInformation("UserName {0} is {1}",userName,result is null? "free":"occupied");
        return Json(result is null ? "true" : $"Name {userName} has already been occupied!");
    }
}