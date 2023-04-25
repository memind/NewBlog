using FluentValidation;
using NewBlog.Entity.Entities;

namespace NewBlog.Service.FluentValidation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name).NotEmpty().NotNull().MinimumLength(3).MaximumLength(100).WithName("Category Name");
        }
    }
}
