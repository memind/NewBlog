using NewBlog.Entity.Entities;

namespace NewBlog.Service.Services.Abstractions
{
    public interface IVisitorService
    {
        Task<List<ArticleVisitor>> GetAllArticleVisitors();
        Task<Visitor> GetVisitorByIp(string ipAddress);
        Task AddArticleVisitor(Article article, ArticleVisitor visitor);
        Task<bool> IsVisitorIncludedInArticle(Guid articleId, int visitorId);
    }
}
