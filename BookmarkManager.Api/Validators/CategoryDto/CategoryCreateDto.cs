using BookmarkManager.Api.DTOs.CategoryDto;
using FluentValidation;
namespace BookmarkManager.Api.Validators.CategoryDto
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            //string Name,
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");
            //string Color
            RuleFor(x => x.Color)
                .NotEmpty()
                .WithMessage("Color is required.")
                .Matches("^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$")
                .WithMessage("Color must be a valid hex code (e.g., #FFF or #FFFFFF).");
        }
    }
}
