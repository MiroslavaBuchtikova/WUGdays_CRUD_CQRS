using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using University.Persistence.Context;
using University.Persistence.Entities.Students;
using University.Services;

namespace University
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            
            services.AddControllersWithViews();
            services.AddTransient<StudentService>();
            
            services.AddRepositories().AddMediatR(Assembly.GetExecutingAssembly());
            var connectionString = Configuration.GetConnectionString("database");
            services.AddDbContextPool<UniversityDbContext>(option =>
                option.UseSqlServer(connectionString,
                    contextOptions =>
                    {
                        contextOptions.MigrationsAssembly(typeof(UniversityDbContext).Assembly.FullName);
                    })
            );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseHealthChecks("/health");

            var runMigration = true;
            if (runMigration)
            {
                using var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();
                using var context = serviceScope.ServiceProvider.GetService<UniversityDbContext>();
                context.Database.Migrate();
                SeedDatabase(context);
            }
        }

        private static void SeedDatabase(UniversityDbContext context)
        {
            var courses = new List<Course>()
            {
                new()
                {
                    Name = "Calculus",
                    Credits = 3
                },
                new()
                {
                    Name = "Chemistry",
                    Credits = 3
                },
                new()
                {
                    Name = "Composition",
                    Credits = 3
                },
                new()
                {
                    Name = "Literature",
                    Credits = 4
                },
                new()
                {
                    Name = "Trigonometry",
                    Credits = 4
                },
                new()
                {
                    Name = "Microeconomics",
                    Credits = 3
                },
                new()
                {
                    Name = "Macroeconomics",
                    Credits = 3
                },
            };

            if (!context.Courses.Any())
            {
                context.Courses.AddRange(courses);
                context.SaveChanges();
            }
        }
    }
}