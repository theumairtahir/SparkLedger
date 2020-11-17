using FluentValidation;
using System.Linq;

namespace TechFlurry.SparkLedger.Validations.CommonValidations
{

    internal static class PhoneNumberValidator
    {
        public static IValidator<string> GetValidator()
        {
            return new Validator();
        }
        class Validator : AbstractValidator<string>
        {
            public Validator()
            {
                RuleFor(x => x).Length(13).WithMessage("Phone number is too short")
                                .Must(x =>
                                {
                                    return !string.IsNullOrEmpty(x) && x.Substring(0, 1) == "+";
                                })
                                .Must(x =>
                                {
                                    return !string.IsNullOrEmpty(x) && x.Substring(1).All(y => y >= 48 && y <= 57);
                                }).WithMessage("Not a valid phone number (e.g. +923XXXXXXXXX)");
            }
        }
    }
}
