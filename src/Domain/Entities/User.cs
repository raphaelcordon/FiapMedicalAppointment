using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}