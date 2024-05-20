using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class MedicalSpecialty
    {
        public Guid Id { get; set; }
        public string Specialty { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();
    }
}