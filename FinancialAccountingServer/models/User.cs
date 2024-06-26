﻿using System.Data;

namespace FinancialAccountingServer.models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Role { get; set; } = "Client";

        public string? AvatarPath { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<GroupMembership> Memberships { get; set; }
    }
}
