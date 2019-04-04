using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
//using testWebApp.Models.Extend;
using testWebApp.Models;
//using testWebApp.Models.Extend;

namespace testWebApp.Controllers
{
    public class UserController : Controller
    {
        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "VerifyAcc, ActivationCode")] Table tableUser) 
            // protect from user 
        {
            bool status = false;
            string message = "";

            // model validation
            if (ModelState.IsValid)
            {
                /*#region Email exist
                // email exist?
                var isExist = isEmailExist(tableUser.Email);

                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already Exist");
                    return View(tableUser);
                }
                #endregion*/

                #region generate activation code
                tableUser.ActivationCode = Guid.NewGuid();
                #endregion

                #region password hashing 
                tableUser.Password = Crypto.Hashing(tableUser.Password);
                #endregion

                tableUser.VerifyAcc = false;

                #region save to DB and send verify
                using (MyDBEntities dc = new MyDBEntities())
                {
                    dc.User.Add(tableUser);
                    dc.SaveChanges();

                    // send verify e-mail
                    SenderVerifyLink(tableUser.Email, tableUser.ActivationCode.ToString());
                    message = "Registration successfully. Activation link " +
                        "sent to email: " + tableUser.Email;
                    status = true;
                }
                #endregion 


            }
            else
            {
                message = "Invalid request";
            }

            ViewBag.message = message;
            
            ViewBag.status = status;
            
            return View();
        }



        /*[NonAction]
        public bool isEmailExist(string Email)
        {
            using (MyDBEntities dc = new MyDBEntities())
            {
                var v = dc.User.Where(a => a.Email == Email).FirstOrDefault();
                return v != null;
                //return View(v);
            }
        }*/

        [NonAction]
        public void SenderVerifyLink(string Email, string activationCode)
        {
            var verifyUrl = "/User/VerifyAcc/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("ameengulistan@gmail.com", "validate email");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "149301hAzard";
            string subject = "Your account created";

            string body = "<br/><br/>Your account is completed. Please follow this link to verify your account"
                + "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "Smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) 

            smtp.Send(message);
            
        }
    }
}