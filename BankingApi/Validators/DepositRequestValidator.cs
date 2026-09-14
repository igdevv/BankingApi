using BankingApi.Dtos.Accounts;
using FluentValidation;

namespace BankingApi.Validators
{
    public class DepositRequestValidator: AbstractValidator<DepositRequest>
    {
        public DepositRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
