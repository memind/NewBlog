using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewBlog.Entity.DTOs.Articles;
using NewBlog.Entity.Entities;
using NewBlog.Service.Extensions;
using NewBlog.Service.Services.Abstractions;
using NewBlog.Web.Consts;
using NewBlog.Web.ResultMessages;
using NToastNotify;

namespace NewBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Article> _validator;
        private readonly IToastNotification _toast;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IValidator<Article> validator, IToastNotification toast)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
            _toast = toast;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}, {RoleConsts.User}")]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> DeletedArticles()
        {
            var articles = await _articleService.GetAllArticlesWithCategoryDeletedAsync();
            return View(articles);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeleted();
            return View(new ArticleAddDto { Categories = categories });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(ArticleAddDto article)
        {
            var map = _mapper.Map<Article>(article);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _articleService.CreateArticleAsync(article);
                _toast.AddSuccessToastMessage(Messages.ArticleMessage.Add(article.Title), new ToastrOptions { Title = "Creating Article"});
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(ModelState);
                var categories = await _categoryService.GetAllCategoriesNonDeleted();
                return View(new ArticleAddDto { Categories = categories });
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
            var categories = await _categoryService.GetAllCategoriesNonDeleted();

            var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
            articleUpdateDto.Categories = categories;

            return View(articleUpdateDto);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(ArticleUpdateDto article)
        {
            var map = _mapper.Map<Article>(article);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await _articleService.UpdateArticleAsync(article);
                _toast.AddSuccessToastMessage(Messages.ArticleMessage.Update(title), new ToastrOptions() { Title = "Updating Article" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
                result.AddToModelState(ModelState);


            var categories = await _categoryService.GetAllCategoriesNonDeleted();
            article.Categories = categories;
            return View(article);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            var title = await _articleService.SafeDeleteArticleAsync(articleId);
            _toast.AddSuccessToastMessage(Messages.ArticleMessage.Delete(title), new ToastrOptions() { Title = "Deleting Article" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }

        [Authorize(Roles = $"{RoleConsts.SuperAdmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            var title = await _articleService.UndoDeleteArticleAsync(articleId);
            _toast.AddSuccessToastMessage(Messages.ArticleMessage.UndoDelete(title), new ToastrOptions() { Title = "Restoring Article" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}
