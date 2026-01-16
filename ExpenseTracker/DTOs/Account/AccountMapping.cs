namespace ExpenseTracker.Api.DTOs.Account
{
    public static class AccountMapping
    {
        public static AccountDto ToDto(this Entities.Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                Name = account.Name,
                Currency = account.Currency.ToString(),
                Balance = account.Balance
            };
        }

        public static Entities.Account ToEntity(this CreateAccountDto createAccountDto, string userId)
        {
            return new Entities.Account
            {
                Id = $"a_{Guid.CreateVersion7()}",
                UserId = userId,
                Name = createAccountDto.Name,
                Currency = createAccountDto.Currency,
                Balance = createAccountDto.InitialBalance
            };
        }

        public static void UpdateFromDto(this Entities.Account account, AccountPatchDto updateAccountDto)
        {
            if(updateAccountDto.Name is not null)
                account.Name = updateAccountDto.Name;
        }
    }
}
