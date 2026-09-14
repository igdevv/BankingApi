using BankingApi.Dtos.Transfers;
using FluentValidation;

namespace BankingApi.Validators
{
    public class CreateTransferRequestValidator: AbstractValidator<CreateTransferRequest>
    {
        public CreateTransferRequestValidator()
        {
            RuleFor(x => x.SourceAccountId)
            .GreaterThan(0);

            RuleFor(x => x.DestinationAccountId)
                .GreaterThan(0);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
