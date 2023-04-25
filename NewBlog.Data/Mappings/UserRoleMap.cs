using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewBlog.Entity.Entities;

namespace NewBlog.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });

            builder.ToTable("AspNetUserRoles");

            builder.HasData
            (
                new AppUserRole
                {
                    UserId = Guid.Parse("DE325EB3-2537-43AA-A629-2477F380EBFE"),
                    RoleId = Guid.Parse("EE17E4E2-C504-42EC-84E8-7DC83E8B2E0B")
                },
                new AppUserRole
                {
                    UserId = Guid.Parse("3727E85C-88D4-4ED7-B5BC-9409B69B4C14"),
                    RoleId = Guid.Parse("BA8E454A-8419-4D0A-B404-83457E34945A")
                },
                new AppUserRole
                {
                    UserId = Guid.Parse("271D524B-B9A7-46C8-8D1C-D9230B4C4186"),
                    RoleId = Guid.Parse("EB9E35A0-F00D-4EB7-9674-B0E9ADB6D809")
                }

            );
        }
    }
}
