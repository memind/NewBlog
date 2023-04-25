using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewBlog.Entity.DTOs.Categories;
using NewBlog.Entity.Entities;
using NewBlog.Service.Services.Abstractions;
using NewBlog.Web.Consts;
using NToastNotify;
using static NewBlog.Web.ResultMessages.Messages;

namespace NewBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<Category> _validator;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toast;

        public CategoryController(ICategoryService categoryService, IValidator<Category> validator, IMapper mapper, IToastNotification toast)
        {
            _categoryService = categoryService;
            _validator = validator;
            _mapper = mapper;
            _toast = toast;
        }

        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}, {RoleConsts.User}")]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeleted();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> DeletedCategories()
        {
            var categories = await _categoryService.GetAllCategoriesDeleted();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(CategoryAddDto category)
        {
            var map = _mapper.Map<Category>(category);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);
                _toast.AddSuccessToastMessage(CategoryMessage.Add(category.Name), new ToastrOptions { Title = "Creating Category" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(ModelState);
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDto categoryAddDto)
        {
            var map = _mapper.Map<Category>(categoryAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _categoryService.CreateCategoryAsync(categoryAddDto);
                _toast.AddSuccessToastMessage(CategoryMessage.Add(categoryAddDto.Name), new ToastrOptions { Title = "Creating Category" });
                return Json(CategoryMessage.Add(categoryAddDto.Name));
            }
            else
            {
                _toast.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "Creating Category" });
                return Json(result.Errors.First().ErrorMessage);
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryByGuid(categoryId);
            var map = _mapper.Map<Category, CategoryUpdateDto>(category);

            return View(map);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(CategoryUpdateDto category)
        {
            var map = _mapper.Map<Category>(category);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await _categoryService.UpdateCategoryAsync(category);
                _toast.AddSuccessToastMessage(CategoryMessage.Update(name), new ToastrOptions { Title = "Updating Category" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(ModelState);
            return View(map);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var title = await _categoryService.SafeDeleteCategoryAsync(categoryId);
            _toast.AddSuccessToastMessage(CategoryMessage.Delete(title), new ToastrOptions() { Title = "Deleting Category" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var title = await _categoryService.UndoDeleteCategoryAsync(categoryId);
            _toast.AddSuccessToastMessage(CategoryMessage.UndoDelete(title), new ToastrOptions() { Title = "Deleting Category" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
    }
}
