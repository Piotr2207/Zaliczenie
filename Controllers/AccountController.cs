
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers;
public class AccountController : Controller
{
    private static readonly HttpClient _http = new(new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (a, b, c, d) => true
    });
	private readonly UserManager<Customer> _userManager;
    private readonly SignInManager<Customer> _signInManager;

    public AccountController(UserManager<Customer> userManager,
                              SignInManager<Customer> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new Customer
            {
                Name = model.Name,
                UserName = model.Username,
                Email = model.Email,
                HasNewsletterSubscribed = false,
                MembershipTypeId = 1,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };

            var createCusRes = await _userManager.CreateAsync(user, model.Password);

            if (createCusRes.Succeeded)
            {
                var addToRoleRes = await _userManager.AddToRoleAsync(user, "User");
                
                if (addToRoleRes.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                } else {
                    ModelState.AddModelError(string.Empty, "Cannot register user");
                }

                
            }

            foreach (var error in createCusRes.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

        }
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel user)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

        }
        return View(user);
    } 

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
