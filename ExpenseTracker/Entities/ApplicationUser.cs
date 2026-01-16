using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace ExpenseTracker.Api.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string DisplayName { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Budget> Budgets { get; set; } = new List<Budget>();
    }
}
