namespace Appointments.Core.Entities;

public class Appointment
{
    public Appointment(DateTime date)
    {
        Date = date;
    }
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; }
    public bool IsApproved { get; set; } = false;
}