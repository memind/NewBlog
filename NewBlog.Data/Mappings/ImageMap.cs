using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewBlog.Entity.Entities;

namespace NewBlog.Data.Mappings
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.FileType).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired(false);
            builder.Property(x => x.ModifiedBy).IsRequired(false);
            builder.Property(x => x.DeletedBy).IsRequired(false);

            builder.HasData
                (
                    new Image
                    {
                        Id = Guid.Parse("8DDA30A9-4D6D-46C2-886E-6B9B050CA9DE"),
                        FileName = "images/testimage",
                        FileType = "jpg",
                        CreatedBy = "Admin Test",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    },
                    new Image
                    {
                        Id = Guid.Parse("CCDBC6D7-6E60-473F-91DA-A85082D67D00"),
                        FileName = "images/vstest",
                        FileType = "png",
                        CreatedBy = "Admin Test",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    },
                    new Image
                    {
                        Id = Guid.Parse("4612419C-ED89-4F4E-B75D-7261688F0E45"),
                        FileName = "images/vstest",
                        FileType = "png",
                        CreatedBy = "Admin Test",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    }
                );
        }
    }
}
