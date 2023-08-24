using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ModernisationChallenge.DataAccess.NetCore
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Task> Tasks { get; set; }

        public DataContext() { }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            SetupBaseData();

            return base.SaveChanges();
        }

        private void SetupBaseData()
        {
            IEnumerable<EntityEntry>? entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                     (e.State == EntityState.Added ||
                      e.State == EntityState.Modified));

            foreach (EntityEntry? entityEntry in entries)
            {
                BaseEntity? baseEntity = (BaseEntity)entityEntry.Entity;
                baseEntity.DateModified = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    baseEntity.DateCreated = DateTime.Now;
                }
            }
        }
    }
}
