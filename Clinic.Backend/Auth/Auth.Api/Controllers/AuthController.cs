using Auth.Api.Models.Auth;
using Auth.Core.Interface.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    
    [HttpPost]
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

        return RedirectToAction("EmailVerification");
    }

    [HttpGet]
    public async Task<IActionResult> SignIn(string? returnUrl)
    {
        var signInModel = new SignInModel
        {
            ReturnUrl = returnUrl.IsNullOrEmpty() ? "https://localhost:5005" : returnUrl
        };
        return View(signInModel);
    }

    [HttpPost]
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
        

        return Redirect(signInModel.ReturnUrl);
    }
    
    public IActionResult EmailVerification() => View();
    
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        await _authService.ConfirmEmailAsync(email, token);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}