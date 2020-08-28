﻿using BugTracker_1._1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BugTracker_1._1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = $"<p>Email From: <bold>{model.FromName}</bold> ({model.FromEmail})</p><p>Message:</p><p>{model.Body}</p>";
                    var from = $"{model.FromEmail}";
                    model.Body = "This is a message from your portfolio site.  The name and the email of the contacting person is above.";
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = $"Portfolio Email: {model.FromEmail}.",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };
                    var svc = new EmailService();
                    await svc.SendAsync(email);
                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            
            return View(model);
        }
    }
}