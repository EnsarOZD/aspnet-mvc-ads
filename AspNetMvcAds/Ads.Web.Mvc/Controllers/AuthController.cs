using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ads.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Azure.Core;
using Ads.Services.Services;

namespace Ads.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext DbContext;
        private readonly EmailService _emailService;
        private readonly TokenUsageService _tokenUsageService;

        public AuthController(AppDbContext dbContext, EmailService emailService, TokenUsageService tokenUsageService)
        {
            DbContext = dbContext;
            _emailService = emailService;
            _tokenUsageService = tokenUsageService;
        }


        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Girdiğiniz değerleri kontrol ediniz";
                return View();
            }
            var existingUser = await DbContext.UserEntities.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresi zaten kullanımda.");
                return View(model);
            }

            if (model.Password != model.PasswordVerify)
            {
                ViewBag.Error = "Şifreler uyuşmuyor";
                return View(model);
            }

            UserEntity user = new()
            {
                Email = model.Email,
                Password = model.Password,
                Roles = "User",
                IsEmailConfirmed = false,
                EmailConfirmationToken = Guid.NewGuid().ToString("n").Substring(0, 6).ToUpper()
            };

            DbContext.UserEntities.Add(user);
            await DbContext.SaveChangesAsync();

            string mailMessage = $" Thank you for registration to our website.  Here your verification code is: <strong>{user.EmailConfirmationToken}</strong>";

            await _emailService.SendEmailAsync(model.Email, "User Verification", mailMessage);

            ModelState.Clear();
            return View("~/Views/Auth/VerifyAccount.cshtml", user.Id);
        }

        [HttpGet]
        public IActionResult VerifyAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAccount([FromForm] string verificationCode, [FromRoute] int id)
        {
            var user = await DbContext.UserEntities.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.IsEmailConfirmed)
            {
                ViewBag.HasMessage = true;
                ViewBag.Error = "Hesap zaten onaylanmış";
                return View();
            }

            if (user.EmailConfirmationToken != verificationCode)
            {
                ViewBag.HasMessage = true;
                ViewBag.Error = "Onay kodu hatalı";
                return View();
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            DbContext.UserEntities.Update(user);
            await DbContext.SaveChangesAsync();

            ViewBag.HasMessage = true;
            ViewBag.Success = "Hesabınız onaylandı";
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string redirectUrl)
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel login, [FromForm] string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await DbContext.UserEntities.SingleOrDefaultAsync(x => x.Email == login.Email
                && x.Password == login.Password
                && x.IsEmailConfirmed);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                return View(login);
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
            };

            user.Roles.Split(",", StringSplitOptions.TrimEntries).ToList().ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = login.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            if (returnUrl is not null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            string? email = model.Email;
            var user = await DbContext.UserEntities.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {

                user.PasswordResetToken = _tokenUsageService.CreateRandomToken();
                TempData["PasswordResetTokenforGetMethod"] = user.PasswordResetToken;
                TempData["PasswordResetTokenforPostMethod"] = user.PasswordResetToken;
                user.ResetTokenExpires = DateTime.Now.AddDays(1);
                await DbContext.SaveChangesAsync();

                PasswordResetService resetService = new PasswordResetService(_emailService);
                resetService.SendPasswordResetEmail(user.Email, user.PasswordResetToken);

                ViewBag.Message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
                return View();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresi kayıtlı değil.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request, [FromRoute] string id)
        {

            request.Token = TempData["PasswordResetTokenforGetMethod"] as string;
            var user = DbContext.UserEntities.FirstOrDefault(u => u.PasswordResetToken == request.Token);

            if (id != request.Token)
            {
                return BadRequest("Invalid Token.");
            }
            return View();
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Error = "Girdiğiniz şifreleri kontrol ediniz";
            //    return View();
            //}
            request.Token = TempData["PasswordResetTokenforPostMethod"] as string;

            var user = await DbContext.UserEntities.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now || user.PasswordResetToken != request.Token)
            {
                return BadRequest("Invalid Token.");
            }

            _tokenUsageService.CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            if (request.NewPassword == request.ConfirmNewPassword)
            {

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Password = request.NewPassword;
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;
                await DbContext.SaveChangesAsync();
                ViewBag.Success = "Your password changed succesfully!";
                return View();

            }
            else
            {
                ViewBag.Error = "Passwords does not match! Please request a new password link!";
            }

            return View();
        }


    }
}
