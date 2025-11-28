using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.Domain.Entities;

namespace MiniBiblioteca.Infrastructure.Data
{
    public class BibliotecaDbContext : DbContext
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Aluguel> Alugueis { get; set; }
        public DbSet<Reserva> Reservas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.idUsuario);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(14);
                entity.HasIndex(e => e.Cpf).IsUnique();
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.Tipo).HasConversion<int>();
                entity.Property(e => e.Ativo).HasDefaultValue(true);
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.idLivro);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Autor).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Categoria).HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(2000);
                entity.Property(e => e.ImagemCapa).HasMaxLength(500);
                entity.Property(e => e.QuantidadeTotal).IsRequired();
                entity.Property(e => e.QuantidadeDisponivel).IsRequired();
                entity.Property(e => e.Ativo).HasDefaultValue(true);
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Aluguel>(entity =>
            {
                entity.HasKey(e => e.idAluguel);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Alugueis)
                      .HasForeignKey(e => e.idUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Livro)
                      .WithMany(l => l.Alugueis)
                      .HasForeignKey(e => e.idLivro)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.DataAluguel).IsRequired();
                entity.Property(e => e.DataPrevistaDevolucao).IsRequired();
                entity.Property(e => e.DiasAluguel).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>().IsRequired();
                entity.Property(e => e.ValorMulta).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ValorTotal).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Observacoes).HasMaxLength(500);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.idReserva);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Reservas)
                      .HasForeignKey(e => e.idUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Livro)
                      .WithMany(l => l.Reservas)
                      .HasForeignKey(e => e.idLivro)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.DataReserva).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>().IsRequired();
                entity.Property(e => e.Observacoes).HasMaxLength(500);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
