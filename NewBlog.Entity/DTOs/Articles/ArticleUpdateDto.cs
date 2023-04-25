using Microsoft.AspNetCore.Http;
using NewBlog.Entity.DTOs.Categories;
using NewBlog.Entity.Entities;

namespace NewBlog.Entity.DTOs.Articles
{
    public class ArticleUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Image? Image { get; set; }
        public IFormFile? Photo { get; set; }
        public Guid CategoryId { get; set; }
        public IList<CategoryDto> Categories { get; set; }
    }
}
