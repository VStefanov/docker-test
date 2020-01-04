using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestApp.Api
{
    public class IndexModel
    {
        public int Id { get; set; }
        public string Index { get; set; }
    }
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString= "Host=<host>;Port=<port>;Username=<user>;Password=<pass>;Database=<database>;";

            connectionString = connectionString.Replace("<host>", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
                                              .Replace("<port>", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
                                              .Replace("<user>", Environment.GetEnvironmentVariable("POSTGRES_USER"))
                                              .Replace("<pass>", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
                                              .Replace("<database>", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"));

            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<IndexModel> Indexes { get; set; }
    }
}
