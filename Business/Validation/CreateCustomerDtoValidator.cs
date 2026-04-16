using FluentValidation;
using SalesManagementAPI.Core.DTOs.Customers;

namespace SalesManagementAPI.Business.Validation
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator() 
        {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Müşteri adı boş olamaz.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon boş olamaz")
                .Length(10, 11).WithMessage("Telefon numarası 10 veya 11 karakter olmalıdır.")
                .Matches(@"^\d+$").WithMessage("Telefon numarası sadece rakamlardan oluşmalıdır.");

        }
    }
}
