using Auth.Api.Models.Auth;
using Auth.Core.Logic.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;


public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpModel signUpModel)
    {
        if (!ModelState.IsValid)
        {
            return View(signUpModel);
        }

        var result = await _authService.SignUpAsync(signUpModel.Email, signUpModel.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return View(signUpModel);
        }

        return RedirectToAction("Index", "Home");
    }
    
    
}