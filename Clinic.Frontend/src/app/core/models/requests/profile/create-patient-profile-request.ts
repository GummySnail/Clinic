export interface CreatePatientProfileRequest {
  firstName: string;
  lastName: string;
  middleName: string | null;
  dateOfBirth: string;
  phoneNumber: string;
  photo: File | null;
}
