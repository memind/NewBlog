using NewBlog.Core.Entities;

namespace NewBlog.Entity.Entities
{
    public class Category : BaseEntity
    {
        public Category() { }
        public Category(string name, string createdBy)
        {
            Name = name;
            CreatedBy = createdBy;
        }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
