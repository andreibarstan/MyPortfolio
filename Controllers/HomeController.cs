using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyPortfolio.Data;
using MyPortfolio.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;


namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        //[Authorize]  (Roles="Admin") prohibits non-admin user from accessing unauthorised routes 
        public  IActionResult Index() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(SendMailDTO sendMailDTO)
        {
            if(!ModelState.IsValid)
                return View();
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(sendMailDTO.Email);

                string myEmailAddress = "andreibarstan@gmail.com"; //host email and password
                string gmailPassword = "pupaequcmukqradr"; // for gmail enable app passwords (can be config in gmail settings)

                mail.To.Add (myEmailAddress);
                mail.Subject = sendMailDTO.Subject;
                mail.IsBodyHtml = true;

                string content = "Name: " + sendMailDTO.Name;
                content += "<br/> Sender email address: " + sendMailDTO.Email;
                content += "<br/> Message: " + sendMailDTO.Message;
                mail.Body = content;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                
                NetworkCredential networkCredential = new NetworkCredential(myEmailAddress, gmailPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);

                ViewBag.Message = "Email sent successfully!";
                ModelState.Clear();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
            }
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
