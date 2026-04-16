using FluentValidation;
using SalesManagementAPI.Core.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagementAPI.Business.Validation
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .Length(2, 100).WithMessage("İsim alanı çok uzun, lütfen kısaltın.(2 ila 100 karakter arası giriş yapınız.)");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olamalıdır.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stok negatif değer olamaz.");
        }
    }
}
