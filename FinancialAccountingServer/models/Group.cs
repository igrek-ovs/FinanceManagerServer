using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace FinancialAccountingServer.models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        [JsonIgnore]
        public ICollection<GroupMembership> Members { get; set; }
        public ICollection<ExpenseCategory> ExpenseCategories { get; set; }
    }
}
