using Microsoft.AspNetCore.Mvc;
using NewBlog.Service.Services.Abstractions;

namespace NewBlog.Web.ViewComponents
{
    public class HomeCategories : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public HomeCategories(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllCategoriesDeletedTake24();
            return View(categories);
        }
    }
}
