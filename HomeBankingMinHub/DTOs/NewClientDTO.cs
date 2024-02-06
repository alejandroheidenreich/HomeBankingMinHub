﻿using HomeBankingMinHub.Models;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub.DTOs
{
    public class NewClientDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

    }
}
