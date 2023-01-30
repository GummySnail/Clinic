namespace Appointments.Core.Entities;

public class Appointment
{
    public Appointment(string doctorId, string serviceId, string patientId, DateTime date)
    {
        DoctorId = doctorId;
        ServiceId = serviceId;
        PatientId = patientId;
        Date = date;
    }
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; }
    public bool IsApproved { get; set; } = false;
    public Result Result { get; set; }
    public string DoctorId { get; set; }
    public string ServiceId { get; set; }
    public string PatientId { get; set; }
}