using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Shared.EF
{
    internal class EFContext : DbContext
    {
        private readonly Assembly[] _mappingAssemblies;

        public EFContext(DbContextOptions options, Assembly[] mappingAssemblies) : base(options)
        {
            _mappingAssemblies = mappingAssemblies;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var mappingAssembly in _mappingAssemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(mappingAssembly);
            }
        }
    }
}
