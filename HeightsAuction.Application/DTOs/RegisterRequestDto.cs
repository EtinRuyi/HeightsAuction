﻿using HeightsAuction.Domain.Enums;

namespace HeightsAuction.Application.DTOs
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
