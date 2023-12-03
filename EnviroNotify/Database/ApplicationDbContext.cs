using EnviroNotify.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnviroNotify.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> aDbContextOptions) : base(aDbContextOptions)
    {
        
    }

    public DbSet<PersistedClient> PersistedClients { get; set; }
}