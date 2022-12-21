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

        if (!string.IsNullOrEmpty(result))
        {
            ModelState.AddModelError(String.Empty, result);
            return View(signUpModel);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> SignIn(string? returnUrl)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            return View(signInModel);
        }

        var result = await _authService.SignInAsync(signInModel.Email, signInModel.Password);
        
        if (!string.IsNullOrEmpty(result))
        {
            ModelState.AddModelError(String.Empty, result);
            return View(signInModel);
        }
        
        return View(signInModel);
    }

}