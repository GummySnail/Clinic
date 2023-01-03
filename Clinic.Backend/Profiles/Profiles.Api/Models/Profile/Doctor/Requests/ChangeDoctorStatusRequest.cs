using Profiles.Core.Enums;

namespace Profiles.Api.Models.Profile.Doctor.Requests;

public record ChangeDoctorStatusRequest(Status Status);