using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using recipe.Models;

namespace recipe.context
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }
    }

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure DbContext
            services.AddDbContext<appDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            // Other service configurations can be added here
        }
    }
}
