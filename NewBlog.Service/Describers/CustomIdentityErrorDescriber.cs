using Microsoft.AspNetCore.Identity;

namespace NewBlog.Service.Describers
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = "DublicateEmail", Description = $"The email {email} registered already." };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = "DuplicateUserName", Description = $"The user name {userName} registered already." };
        }
        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError { Code = "DublicateRoleName", Description = $"The role {role} already exist." };
        }
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = "InvalidEmail", Description = $"The email {email} is invalid." };
        }
        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError { Code = "InvalidRoleName", Description = $"The role {role} is invalid." };
        }
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError { Code = "InvalidUserName", Description = $"The email {userName} is invalid." };
        }
        public override IdentityError PasswordTooShort(int lenght)
        {
            return new IdentityError { Code = "PasswordTooShort", Description = $"Password is too short ({lenght})." };
        }
        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError { Code = "UserAlreadyInRole", Description = $"The user already has the role {role}." };
        }
        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError { Code = "UserNotInRole", Description = $"User does not have the role {role}!" };
        }
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError { Code = "ConcurrencyFailture", Description = "Multiple users tried to modify the same data! Changes will be undone!" };
        }
        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError { Code = "LoginAlreadyAssociated", Description = "This session is already associated with an account." };
        }
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = "PasswordMismatch", Description = "Password information does not match." };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = "PasswordRequiresDigit", Description = "Password must include at least 1 number." };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = "PasswordRequiresNonAlphanumeric", Description = "Password must include at least 2 different symbol character." };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = "PasswordRequiresUpper", Description = "Your password must contain at least 1 capital letter." };
        }
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError { Code = "PasswordRequiresUniqueChars", Description = "Your password must contain at least 1 symbol character." };
        }
        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError { Code = "RecoveryCodeRedemptionFailed", Description = "Account recovery code is invalid!" };
        }
        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError { Code = "UserAlreadyHasPassword", Description = "The user already has a password." };
        }
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = "DefaultError", Description = "An error occured!" };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = "PasswordRequiresLower", Description = "Your password must contain at least 1 lowercase letter." };
        }
        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError { Code = "UserLockoutNotEnabled", Description = "This account is currently locked! Please try again later." };
        }
    }
}
