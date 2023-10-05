using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ads.Data.Entities;
using Ads.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ads.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext DbContext;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext dbContext, EmailService emailService)
        {
            DbContext = dbContext;
            _emailService = emailService;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
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

            string mailMessage = $"Your verification code is: <strong>{user.EmailConfirmationToken}</strong>";

            await _emailService.SendEmailAsync(model.Email, "User Verification", mailMessage);

            ViewBag.Message = "Onay mail'i gönderildi.";
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
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            string email = model.Email;
            var user = DbContext.UserEntities.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                string resetToken = Guid.NewGuid().ToString(); 
                user.PasswordResetToken = resetToken; 
                DbContext.SaveChanges();

                PasswordResetService resetService = new PasswordResetService(_emailService);
                resetService.SendPasswordResetEmail(user.Email, resetToken);

                TempData["Message"] = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresi kayıtlı değil.");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, ResetPasswordViewModel model)
        {
            string email = model.Email;
            var user = DbContext.UserEntities.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                TempData["Error"] = "Geçersiz e-posta adresi.";
                return RedirectToAction("Login");
            }

            user.Password = model.NewPassword;

            DbContext.SaveChanges();

            TempData["Message"] = "Şifre başarıyla sıfırlandı. Yeni şifrenizle giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }


    }
}
