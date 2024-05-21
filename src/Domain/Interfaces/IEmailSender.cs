using Domain.Dtos;

namespace Domain.Interfaces;

public interface IEmailSender
{
    void AppointmentEmailSender(AppointmentDto data);
}