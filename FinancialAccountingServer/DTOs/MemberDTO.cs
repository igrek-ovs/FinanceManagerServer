﻿namespace FinancialAccountingServer.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string? AvatarPath { get; set; }

        public bool IsAdmin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
