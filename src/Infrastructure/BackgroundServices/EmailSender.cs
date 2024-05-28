using System.Net;
using System.Net.Mail;
using Domain.Dtos;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.BackgroundServices
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AppointmentEmailSender(AppointmentDto data)
        {
            try
            {
                var mailString = _configuration.GetSection("EmailService")["MailString"];
                var mailPass = _configuration.GetSection("EmailService")["PassString"];

                if (string.IsNullOrEmpty(data.PatientEmail))
                {
                    throw new ArgumentNullException(nameof(data.PatientEmail), "Patient email cannot be null or empty.");
                }

                var message = new MailMessage
                {
                    From = new MailAddress(mailString!),
                    Subject = $"Medical appointment {data.AppointmentTime}",
                    Body = $"<html><body>" +
                           $"<p>Please remember you have an appointment soon.</p>" +
                           $"<p>Dr. {data.DoctorName} is the doctor who will see you.</p>" +
                           $"<p>The appointment is for the following specialty: <b>{data.Specialty}</b>.</p>" +
                           $"</body></html>",
                    IsBodyHtml = true
                };

                message.To.Add(new MailAddress(data.PatientEmail));

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(mailString, mailPass),
                    EnableSsl = true
                };

                smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("SmtpException has occurred: " + ex.Message);
            }
        }
    }
}
