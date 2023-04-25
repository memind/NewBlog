using FluentValidation;
using NewBlog.Entity.Entities;

namespace NewBlog.Service.FluentValidation
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50).WithName("Name");
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(50).WithName("Last Name");
            RuleFor(x => x.PhoneNumber).NotEmpty().MinimumLength(11).WithName("Phone Number");
        }
    }
}
