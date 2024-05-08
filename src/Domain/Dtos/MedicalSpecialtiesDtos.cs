namespace Domain.Dtos;

public class MedicalSpecialtiesDtos
{
    public class MedicalSpecialtyDto
    {
        public Guid Id { get; set; }
        public string Specialty { get; set; }
    }

    public class CreateMedicalSpecialtyDto
    {
        public string Specialty { get; set; }
    }

    public class UpdateMedicalSpecialtyDto
    {
        public string Specialty { get; set; }
    }
}