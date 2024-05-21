using FluentValidation;

namespace PaymentGateway.Api.Models.Validators;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty()
            .Length(14, 19)
            .Matches("^[0-9]+$").WithMessage("Card number must only contain numeric characters");

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty()
            .InclusiveBetween(1u, 12u);

        RuleFor(x => x.ExpiryYear)
            .NotEmpty()
            .Must(BeAValidYear).WithMessage("Expiry year must be in the future");

        RuleFor(x => x.ExpiryYear)
            .Must((model, expiryYear) => BeAValidExpiryDate(model.ExpiryMonth, expiryYear))
            .WithMessage("Expiry date must be in the future");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3)
            .Must(ValidCurrency).WithMessage("Currency must be a valid ISO code");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThanOrEqualTo(uint.MinValue);

        RuleFor(x => x.Cvv)
            .NotEmpty()
            .Length(3, 4)
            .Matches("^[0-9]+$").WithMessage("CVV must only contain numeric characters");
    }

    private bool BeAValidYear(uint year)
    {
        return year >= DateTime.Now.Year;
    }

    private bool BeAValidExpiryDate(uint month, uint year)
    {
        return new DateTime((int)year, (int)month, 1) > DateTime.UtcNow;
    }

    private bool ValidCurrency(string currencyCode)
    {
        return Enum.IsDefined(typeof(CurrencyCode), currencyCode);
    }
}