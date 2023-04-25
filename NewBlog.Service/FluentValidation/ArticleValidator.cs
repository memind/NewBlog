using FluentValidation;
using NewBlog.Entity.Entities;

namespace NewBlog.Service.FluentValidation
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().MinimumLength(30).MaximumLength(2500).WithName("Title");
            RuleFor(x => x.Content).NotEmpty().NotNull().MinimumLength(30).MaximumLength(2500).WithName("Content");
        }
    }
}
