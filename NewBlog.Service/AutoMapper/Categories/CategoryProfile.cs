using AutoMapper;
using NewBlog.Entity.DTOs.Categories;
using NewBlog.Entity.Entities;

namespace NewBlog.Service.AutoMapper.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<CategoryAddDto,Category>().ReverseMap();
            CreateMap<CategoryUpdateDto,Category>().ReverseMap();
        }
    }
}
