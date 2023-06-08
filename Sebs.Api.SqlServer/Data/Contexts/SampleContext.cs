using Microsoft.EntityFrameworkCore;
using Sebs.Api.SqlServer.Data.Entities;

namespace Sebs.Api.SqlServer.Data.Contexts
{
    public class SampleContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public SampleContext(DbContextOptions<SampleContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<SampleEntity> Samples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("OrderingConnectionString"));
            }
        }
    }
}