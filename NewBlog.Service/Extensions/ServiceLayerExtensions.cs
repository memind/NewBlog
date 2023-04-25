using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NewBlog.Service.FluentValidation;
using NewBlog.Service.Helpers.Images;
using NewBlog.Service.Services.Abstractions;
using NewBlog.Service.Services.Concrete;
using System.Globalization;
using System.Reflection;

namespace NewBlog.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IArticleService,ArticleService>();
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IDashboardService,DashboardService>();
            services.AddScoped<IVisitorService,VisitorService>();

            services.AddScoped<IImageHelper,ImageHelper>();

            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

            services.AddAutoMapper(assembly);

            services.AddControllersWithViews().AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("en-en");
            });

            return services;
        }
    }
}
