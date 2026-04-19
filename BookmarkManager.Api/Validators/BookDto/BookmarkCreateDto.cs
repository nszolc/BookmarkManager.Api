using FluentValidation;
using BookmarkManager.Api.DTOs.BookDto;
using BookmarkManager.Api.Models;
using System;

namespace BookmarkManager.Api.Validators.BookDto
{
    public class BookmarkCreateDtoValidator : AbstractValidator<BookmarkCreateDto>
    {
        public BookmarkCreateDtoValidator() 
        {
           /* Bookmark x = new Bookmark();
            CustomerValidator x = new CustomerValidator();

            ValidationResult result = validator.Validate(x); */

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("URL is required.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

                RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");

            RuleFor(x => x.TagIds)
                .Must(tags => tags == null || tags.All(tagId => tagId > 0))
                .WithMessage("All TagIds must be greater than 0.");
        }
    }
}
