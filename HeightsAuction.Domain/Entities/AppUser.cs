﻿using HeightsAuction.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HeightsAuction.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
