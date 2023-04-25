using NewBlog.Data.UnitOfWorks;
using NewBlog.Entity.Entities;
using NewBlog.Service.Services.Abstractions;

namespace NewBlog.Service.Services.Concrete
{
    public class VisitorService : IVisitorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public VisitorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ArticleVisitor>> GetAllArticleVisitors()
        {
            return await _unitOfWork.GetRepository<ArticleVisitor>().GetAllAsync(null, x => x.Visitor, y => y.Article);
        }

        public async Task<Visitor> GetVisitorByIp(string ipAddress)
        {
            return await _unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAddress == ipAddress);
        }

        public async Task AddArticleVisitor(Article article, ArticleVisitor visitor)
        {
            await _unitOfWork.GetRepository<ArticleVisitor>().AddAsync(visitor);
            article.ViewCount += 1;
            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> IsVisitorIncludedInArticle(Guid articleId, int visitorId)
        {
            var articleVisitors = await GetAllArticleVisitors();

            if (articleVisitors.Any(x => x.VisitorId == visitorId && x.ArticleId == articleId))
                return true;

            else return false;
        }
    }
}
