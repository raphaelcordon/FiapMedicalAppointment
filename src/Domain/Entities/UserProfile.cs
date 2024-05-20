using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class UserProfile : IdentityUser<Guid>, ISoftDeletable
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<MedicalSpecialty> MedicalSpecialties { get; set; } = new List<MedicalSpecialty>();
    }
}