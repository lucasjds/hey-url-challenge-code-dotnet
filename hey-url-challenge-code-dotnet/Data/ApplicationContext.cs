using hey_url_challenge_code_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace HeyUrlChallengeCodeDotnet.Data
{
  public class ApplicationContext : DbContext
  {
    public DbSet<Url> Urls { get; set; }
    public DbSet<VisitLog> VisitLogs { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Url>()
                  .HasMany<VisitLog>(g => g.VisitLogs)
                  .WithOne(x => x.Url)
                  .HasForeignKey(t => t.IdUrl);
    }
  }
}