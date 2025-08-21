using Microsoft.EntityFrameworkCore;
using LibrariesAdministrator.Application.Interfaces;
using LibrariesAdministrator.Application.Services;
using LibrariesAdministrator.Domain.Ports;
using LibrariesAdministrator.Infrastructure;
using LibrariesAdministrator.Infrastructure.Adapters;

namespace LibrariesAdministrator.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConecction")));

            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();

            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ILoanService, LoanService>();

            return services;
        }
    }
}
