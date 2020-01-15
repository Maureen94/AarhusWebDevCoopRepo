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

        //Function that saves the users input as a umbraco content page
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            //Get the current page so we can use it to save the new content we are creating 
            //under here, on the contact page in the backoffice
            GuidUdi currentPageUdi = new GuidUdi(CurrentPage.ContentType.ItemType.ToString(), CurrentPage.Key);
            
            //Create the new content model in the (current) contact us page
            IContent msg = Services.ContentService.CreateContent(model.Subject, currentPageUdi, "message");
            msg.SetValue("messageName", model.Name);
            msg.SetValue("email", model.Email);
            msg.SetValue("subject", model.Subject);
            msg.SetValue("messageContent", model.Message);

            //Save it to the contact us page
            Services.ContentService.Save(msg);

            //Commented because of not valid email service/client
            //    MailMessage message = new MailMessage();
            //    message.To.Add("maureenkoopman9@gmail.com");
            //    message.Subject = model.Subject;
            //    message.From = new MailAddress(model.Email, model.Name);
            //    message.Body = model.Message;

            //    using (SmtpClient smtp = new SmtpClient())
            //    {
            //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //        smtp.UseDefaultCredentials = false;
            //        smtp.EnableSsl = true;
            //        smtp.Host = "smtp.gmail.com";
            //        smtp.Port = 587;
            //        smtp.Credentials = new System.Net.NetworkCredential("", "");

            //        //send mail
            //        smtp.Send(message);}
            //        TempData["Success"] = true;
            //
            return RedirectToCurrentUmbracoPage();
        }


    }

}