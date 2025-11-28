using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MiniBiblioteca.Infrastructure.Data
{
    public class BibliotecaDbContextFactory : IDesignTimeDbContextFactory<BibliotecaDbContext>
    {
        public BibliotecaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BibliotecaDbContext>();

            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MiniBibliotecaDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BibliotecaDbContext(optionsBuilder.Options);
        }
    }
}
