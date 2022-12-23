﻿using Auth.Api.Models.Auth;
using Auth.Core.Logic.Auth;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;


public class AuthController : Controller
{
    private readonly AuthService _authService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IIdentityServerInteractionService _interaction;

    public AuthController(AuthService authService, IHttpClientFactory httpClientFactory, IIdentityServerInteractionService interaction)
    {
        _authService = authService;
        _httpClientFactory = httpClientFactory;
        _interaction = interaction;
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

        return RedirectToAction("EmailVerification");
    }

    [HttpGet]
    public async Task<IActionResult> SignIn(string? returnUrl)
    {
        /*var serverClient = _httpClientFactory.CreateClient();
        var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:5005/");

        var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "client-secret",
                Scope = "UserInfoScope"
            });

        var apiClient = _httpClientFactory.CreateClient();
        
        apiClient.SetBearerToken(tokenResponse.AccessToken);

        var response = await apiClient.GetAsync("https://localhost:5005/secret");

        var content = await response.Content.ReadAsStringAsync();*/
        
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
        

        return Redirect("https://localhost:5005");
    }
    
    public IActionResult EmailVerification() => View();

    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        await _authService.ConfirmEmailAsync(email, token);
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _authService.logoutAsync();
        return RedirectToAction("Index", "Home");
    }
}