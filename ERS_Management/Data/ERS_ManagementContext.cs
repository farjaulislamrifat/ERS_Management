using Microsoft.EntityFrameworkCore;
using ERS_Management.Models;

namespace ERS_Management.Data
{
    public class ERS_ManagementContext : DbContext
    {
        public ERS_ManagementContext(DbContextOptions<ERS_ManagementContext> options)
            : base(options)
        {
        }

        public DbSet<ERS_Management.Models.Account> Account { get; set; } = default!;
        public DbSet<ERS_Management.Models.Entries> Entries { get; set; } = default!;
        public DbSet<ERS_Management.Models.FaultEntry> FaultEntry { get; set; } = default!;

    }
}
