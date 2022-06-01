using Microsoft.EntityFrameworkCore;

namespace MyWebApi.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
        {
        }

        #region
        public DbSet<Goods> Goodss { get; set; }
        public DbSet<Category> Categories { get; set; }
        #endregion
    }

}
