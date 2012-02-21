using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace MyJobLeads.DomainModel.Utilities
{
    /// <summary>
    /// Class that contains utilities for dealing with emails
    /// </summary>
    public class EmailUtils
    {
        /// <summary>
        /// Sends an email with the specified parameters
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        public virtual void Send(string toAddress, string subject, string messageBody, bool isHtml = false)
        {
            // Create a SmtpClient using the parameters from app.config
            var client = new SmtpClient();
            var msg = new MailMessage();

            // Setup the mail message
            msg.To.Add(toAddress);
            msg.Subject = subject;
            msg.Body = messageBody;
            msg.IsBodyHtml = isHtml;

            // Send the email
            client.Send(msg);
        }
    }
}
