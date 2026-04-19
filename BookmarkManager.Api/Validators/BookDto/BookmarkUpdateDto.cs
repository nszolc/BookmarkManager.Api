using FluentValidation;
using BookmarkManager.Api.DTOs.BookDto;


namespace BookmarkManager.Api.Validators.BookDto
{
    public class BookmarkUpdateDtoValidator : AbstractValidator<BookmarkUpdateDto>
    {
        public BookmarkUpdateDtoValidator() 
        {
            //string Title,
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.");
            //string Url,
            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("Url is required.")
                .MaximumLength(500)
                .WithMessage("Url must not exceed 500 characters.");
 
            //string? Description,
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");
            //int CategoryId,
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");
            // List< int >? TagIds
                RuleFor(x => x.TagIds)
                    .Must(tagIds => tagIds == null || tagIds.All(id => id > 0))
                    .WithMessage("All TagIds must be greater than 0.");


        }
    }
}
