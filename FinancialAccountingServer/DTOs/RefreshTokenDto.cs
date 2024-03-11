namespace FinancialAccountingServer.DTOs
{
    public class RefreshTokenDto
    {
        public int UserId { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
