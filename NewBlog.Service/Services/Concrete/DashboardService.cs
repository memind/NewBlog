using NewBlog.Data.UnitOfWorks;
using NewBlog.Entity.Entities;
using NewBlog.Service.Services.Abstractions;

namespace NewBlog.Service.Services.Concrete
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<int>> GetYearlyArticleCounts()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted);
            var startDate = new DateTime(DateTime.Now.Date.Year, 1, 1);

            List<int> datas = new();

            for (int i = 1; i <= 12; i++)
            {
                var startedDate = new DateTime(startDate.Year, i, 1);
                var endedDate = startedDate.AddMonths(1);

                var data = articles.Where(x => x.CreatedDate >= startedDate && x.CreatedDate < endedDate).Count();
                datas.Add(data);
            }

            return datas;
        }

        public async Task<int> GetTotalArticleCount()
        {
            return await _unitOfWork.GetRepository<Article>().CountAsync();
        }

        public async Task<int> GetTotalCategoryCount()
        {
            return await _unitOfWork.GetRepository<Category>().CountAsync();
        }
    }
}
