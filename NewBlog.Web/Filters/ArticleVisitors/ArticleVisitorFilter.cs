using Microsoft.AspNetCore.Mvc.Filters;
using NewBlog.Data.UnitOfWorks;
using NewBlog.Entity.Entities;

namespace NewBlog.Web.Filters.ArticleVisitors
{
    public class ArticleVisitorFilter : IAsyncActionFilter
    {
        private IUnitOfWork _unitOfWork;

        public ArticleVisitorFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            List<Visitor> visitors = _unitOfWork.GetRepository<Visitor>().GetAllAsync().Result;

            string getIp = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string getUserAgent = context.HttpContext.Request.Headers["User-Agent"];

            Visitor visitor = new Visitor(getIp,getUserAgent);

            if (visitors.Any(x => x.IpAddress == visitor.IpAddress))
                return next();

            else
            {
                _unitOfWork.GetRepository<Visitor>().AddAsync(visitor);
                _unitOfWork.Save();
            }

            return next();
        }
    }
}
