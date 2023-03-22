 using CI.Models;
using CI_Entity.CIDbContext;
using CI_Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Forget= CI.Models.FogetModel;
using CI.Repository.Interface;
using Newtonsoft.Json.Linq;

namespace CI.Controllers
{
    public class ForgetController : Controller
    {
        private readonly CiPlatformContext _db;
        private readonly IUserRepository _Idb;
        public ForgetController(CiPlatformContext db, IUserRepository Idb)
        {
            _db = db;
            _Idb = Idb;
        }
        public ActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Forget(Forget model)
        {
            if (ModelState.IsValid)
            {
                //var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);
                var user = _Idb.UserExist(model.Email);
                if (user == null)
                {
                    ViewBag.emailerror = "Email not Exist";
                    return View();
                }

                // Generate a password reset token for the user
                var token = Guid.NewGuid().ToString();

                // Store the token in the password resets table with the user's email
                var passwordReset = new PasswordReset
                {
                    Email = model.Email,
                    Token = token
                };
                _db.PasswordResets.Add(passwordReset);
                _db.SaveChanges();

                // Send an email with the password reset link to the user's email address
                var resetLink = Url.Action("ResetPassword", "Forget", new { email = model.Email, token }, Request.Scheme);
                // Send email to user with reset password link
                // ...
                var fromAddress = new MailAddress("communityempowermentportal@gmail.com", "Community Empowerment Portal");
                var toAddress = new MailAddress(model.Email);
                var subject = "Password reset request";
                var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("communityempowermentportal@gmail.com", "bkdkuisqaxfsjgfp"),
                    EnableSsl = true
                };
                smtpClient.Send(message);

                return RedirectToAction("Forget", "Forget");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string email, string token)
        {
            // var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == email && pr.Token == token);
            var passwordReset = _Idb.PasswordResets(email,  token);
            if (passwordReset == null)
            {
                return RedirectToAction("ResetPassword", "Forget");
            }
            // Pass the email and token to the view for resetting the password
            var model = new ResetpassModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetpassModel rsp)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                //var user = _db.Users.FirstOrDefault(u => u.Email == rsp.Email);
                var user = _Idb.UserExist(rsp.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPassword", "ResetPassword");
                }

                // Find the password reset record by email and token
                //var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == rsp.Email && pr.Token == rsp.Token);
                var passwordReset = _Idb.PasswordResets(rsp.Email,rsp.Token);
                if (passwordReset == null)
                {
                    return RedirectToAction("ResetPassword", "ResetPassword");
                }

                // Update the user's password
                user.Password = rsp.Password;
                _db.SaveChanges();

                // Remove the password reset record from the database
                _db.PasswordResets.Remove(passwordReset);
                _db.SaveChanges();

                return RedirectToAction("Login", "Login");
            }

            return View();
        }












        // GET: ForgetController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ForgetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ForgetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForgetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForgetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ForgetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForgetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForgetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}