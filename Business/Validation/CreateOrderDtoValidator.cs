using FluentValidation;
using SalesManagementAPI.Core.DTOs.Orders;

namespace SalesManagementAPI.Business.Validation
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Geçerli bir müşteri seçilmelidir");

            RuleFor(x=>x.Items)
                .NotNull().WithMessage("Sipariş kalemleri boş olamaz")
                .Must(x=> x != null && x.Any()).WithMessage("Sipariş en az bir ürün içermelidir.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                .GreaterThan(0).WithMessage("Geçerli bir ürün seçilmelidir.");
                item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Ürün adedi en az 1 adet olmalıdır.");
            });
        }
    }
}
