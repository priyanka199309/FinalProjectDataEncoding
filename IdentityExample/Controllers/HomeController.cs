using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityExample.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace IdentityExample.Controllers
{
    public class HomeController:Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser>userManager,
            SignInManager<IdentityUser>signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //for guarding an action
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        //for creating a user
        public IActionResult Login()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username,string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user!=null)
            {
                //sign in user
             var signInresult= _signInManager.PasswordSignInAsync(user, password, false, false);

                if(signInresult.IsCompletedSuccessfully)
                {
                    return RedirectToAction("Index");
                }
            }
            

            //Login functionality

            return RedirectToAction("Index");
        }
        public IActionResult Register()
        {

            return View();
        }
       [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
                
            };
               var result=await _userManager.CreateAsync(user,password);
            if(result.Succeeded)
            {
                var code = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(VerifyEmail),"Home",new { userId = user.Id, code },Request.Scheme,Request.Host.ToString());

                //generation of email token
                await _emailService.SendAsync("test@test.com", "email verify",$"<a href=\"{link}\">Verify Email</a>",true);

            
                //sign in user
                //var signInresult = _signInManager.PasswordSignInAsync(user, password, false, false);

                //if (signInresult.IsCompletedSuccessfully)
                //{
                  return RedirectToAction("EmailVerification");
                }
            
            //register functionality
            return RedirectToAction("Index");
        }
       public async Task<IActionResult>VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user,code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }
        public IActionResult EmailVerification() => View();
        
        public async Task<IActionResult>LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
