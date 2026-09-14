using BankingApi.Dtos.Accounts;
using FluentValidation;

namespace BankingApi.Validators
{
    public class WithdrawRequestValidator: AbstractValidator<WithdrawRequest>
    {
        public WithdrawRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
