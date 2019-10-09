using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            //Get the GuildUdi of the cuttent page
            GuidUdi currentPageUdi = new GuidUdi(CurrentPage.ContentType.ItemType.ToString(), CurrentPage.Key);

            //Create the new contenct type 
            IContent msg = Services.ContentService.CreateContent(model.Subject, currentPageUdi, "message");
            msg.SetValue("messageName", model.Name);
            msg.SetValue("email", model.Email);
            msg.SetValue("subject", model.Subject);
            msg.SetValue("messageContent", model.Message);
            msg.SetValue("umbracoNaviHide", true);

            //Save 
            Services.ContentService.Save(msg);


            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            MailMessage message = new MailMessage();
            message.To.Add("maureenkoopman9@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("", "");

                //send mail
                smtp.Send(message);}
                TempData["Success"] = true;
                return RedirectToCurrentUmbracoPage();
        }




    }

}