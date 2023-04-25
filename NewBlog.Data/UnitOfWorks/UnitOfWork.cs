using NewBlog.Data.Context;
using NewBlog.Data.Repositories.Abstraction;
using NewBlog.Data.Repositories.Concretes;

namespace NewBlog.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;

        public UnitOfWork(AppDbContext ctx)
        {
            this._ctx = ctx;
        }

        public async ValueTask DisposeAsync()
        {
            await _ctx.DisposeAsync();
        }

        public int Save()
        {
            return _ctx.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _ctx.SaveChangesAsync();
        }

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(_ctx);
        }
    }
}
