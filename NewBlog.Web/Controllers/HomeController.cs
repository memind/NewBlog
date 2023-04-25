using Microsoft.AspNetCore.Mvc;
using NewBlog.Entity.Entities;
using NewBlog.Service.Services.Abstractions;

namespace NewBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IVisitorService _visitorService;

        public HomeController(IArticleService articleService, IHttpContextAccessor accessor, IVisitorService visitorService)
        {
            _articleService = articleService;
            _accessor = accessor;
            _visitorService = visitorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var articles = await _articleService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending);
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var articles = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var article = await _articleService.GetArticleObject(id);
            var visitor = await _visitorService.GetVisitorByIp(ipAddress);

            var addArticleVisitor = new ArticleVisitor(article.Id, visitor.Id);

            var result = await _articleService.GetArticleWithCategoryNonDeletedAsync(id);

            if (await _visitorService.IsVisitorIncludedInArticle(article.Id, visitor.Id))
                return View(result);

            else
                await _visitorService.AddArticleVisitor(article, addArticleVisitor);

            return View(result);
        }
    }
}