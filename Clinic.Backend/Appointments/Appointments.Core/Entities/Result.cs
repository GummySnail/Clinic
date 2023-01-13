namespace Appointments.Core.Entities;

public class Result
{
    public Result(string complaints, string conclusion, string recommendations, string appointmentId)
    {
        Complaints = complaints;
        Conclusion = conclusion;
        Recommendations = recommendations;
        AppointmentId = appointmentId;
    }
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Complaints { get; set; }
    public string Conclusion { get; set; }
    public string Recommendations { get; set; }
    public Appointment Appointment { get; set; }
    public string AppointmentId { get; set; }
}