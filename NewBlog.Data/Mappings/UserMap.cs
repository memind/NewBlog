using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewBlog.Entity.Entities;

namespace NewBlog.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

            builder.ToTable("AspNetUsers");

            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);

            builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            var superAdmin = new AppUser
            {
                Id = Guid.Parse("DE325EB3-2537-43AA-A629-2477F380EBFE"),
                UserName = "superadmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                PhoneNumber = "+905439999999",
                FirstName = "Emin",
                LastName = "Dağ",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ImageId = Guid.Parse("8DDA30A9-4D6D-46C2-886E-6B9B050CA9DE")
            };
            superAdmin.PasswordHash = CreatePasswordHash(superAdmin, "superadmin");

            var admin = new AppUser
            {
                Id = Guid.Parse("3727E85C-88D4-4ED7-B5BC-9409B69B4C14"),
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PhoneNumber = "+905438888888",
                FirstName = "Ece Nur",
                LastName = "Çelik",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ImageId = Guid.Parse("4612419C-ED89-4F4E-B75D-7261688F0E45")
            };
            admin.PasswordHash = CreatePasswordHash(admin, "admin");

            var user = new AppUser
            {
                Id = Guid.Parse("271D524B-B9A7-46C8-8D1C-D9230B4C4186"),
                UserName = "user@gmail.com",
                NormalizedUserName = "user@GMAIL.COM",
                Email = "user@gmail.com",
                NormalizedEmail = "user@GMAIL.COM",
                PhoneNumber = "+905437777777",
                FirstName = "Nur",
                LastName = "Dağ",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ImageId = Guid.Parse("CCDBC6D7-6E60-473F-91DA-A85082D67D00")
            };
            user.PasswordHash = CreatePasswordHash(user, "user");

            builder.HasData(superAdmin, admin, user);
        }

        private string CreatePasswordHash(AppUser user, string password)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
