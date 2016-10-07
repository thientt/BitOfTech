using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Configuration;
using SendGrid;

namespace BitOfTech.WebApi22.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridAsync(message);
        }

        private async Task configSendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();

            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress("tranthiencdsp@gmail.com", "Tran Thuong Thien");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
                                                    ConfigurationManager.AppSettings["emailService:Password"]);

            // Create a Web transport for sending email.
            string apiKey = "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";
            //var transportWeb = new Web(credentials);
            var transportWeb = new Web(apiKey);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }
}