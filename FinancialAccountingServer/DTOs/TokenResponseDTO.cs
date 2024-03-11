using FinancialAccountingServer.models;

namespace FinancialAccountingServer.DTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
