using System.Net;
using System.Net.Mail;
using Domain.Dtos;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.BackgroundServices;

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

            var message = new MailMessage();
            message.From = new MailAddress(mailString!);
            message.Subject = $"Medical appointment {data.AppointmentTime}";
            message.To.Add(new MailAddress(data.PatientName));
            message.Body = $"<html><body>" +
                           $"<p>Please remember you have an appointment soon.</p>" +
                           $"<p>Dr. {data.DoctorName} is the doctor who will see you.</p>" +
                           $"<p>The appointment is for the following specialty: <b>{data.Specialty}</b>.</p>" +
                           $"{data.PatientName}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(mailString, mailPass),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
        catch (SmtpException ex)
        {
            throw new ApplicationException
                ("SmtpException has occured: " + ex.Message);
        }
    }
}