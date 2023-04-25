using Microsoft.AspNetCore.Identity;
using NewBlog.Core.Entities;

namespace NewBlog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("96799377-f587-48ca-90e8-509dccff5fa8");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
