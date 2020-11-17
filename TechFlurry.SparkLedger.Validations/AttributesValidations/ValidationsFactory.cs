using FluentValidation;
using TechFlurry.SparkLedger.Validations.CommonValidations;

namespace TechFlurry.SparkLedger.Validations.AttributesValidations
{
    public static class ValidationsFactory
    {
        public static IValidator<T> GetValidator<T>(ValidationAttributes attributeName)
        {
            IValidator<T> validator = null;
            switch (attributeName)
            {
                case ValidationAttributes.LedgerAccountTitle:
                    validator = new LedgerAccountTitleValidator<T>();
                    break;
                case ValidationAttributes.AccountHolderPhone:
                    validator = new AccountHolderPhoneValidator<T>();
                    break;
            }
            return validator;
        }
    }
    public enum ValidationAttributes
    {
        LedgerAccountTitle,
        AccountHolderPhone
    }
    class LedgerAccountTitleValidator<T> : AbstractValidator<T>
    {
        public LedgerAccountTitleValidator()
        {
            RuleFor(x => x).NotNull().NotEmpty().WithMessage("Account Title is required");
        }
    }
    class AccountHolderPhoneValidator<T> : AbstractValidator<T>
    {
        public AccountHolderPhoneValidator()
        {
            RuleFor(x => x).NotNull().NotEmpty().WithMessage("Account holder's phone is required")
                            .SetValidator((IValidator<T>)PhoneNumberValidator.GetValidator());
        }
    }
}
