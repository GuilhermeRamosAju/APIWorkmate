using APIWorkmate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace APIWorkmate.Context;

public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Contratacao> Contratacoes { get; set; }
    public DbSet<Mensagem> Mensagens { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Subcategoria> Subcategorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mensagem>()
            .HasOne(m => m.Remetente)
            .WithMany(u => u.MensagensEnviadas)
            .HasForeignKey(m => m.RemetenteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mensagem>()
            .HasOne(m => m.Destinatario)
            .WithMany(u => u.MensagensRecebidas)
            .HasForeignKey(m => m.DestinatarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Tipo)
            .HasConversion<string>();

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Especialidades)
            .WithMany(s => s.Usuarios)
            .UsingEntity<Dictionary<string, object>>(
                "UsuarioSubcategorias",
                j => j.HasOne<Subcategoria>().WithMany().HasForeignKey("SubcategoriaId").HasPrincipalKey("Id"),
                j => j.HasOne<Usuario>().WithMany().HasForeignKey("UsuarioId").HasPrincipalKey("Id"),
                j =>
                {
                    j.HasKey("UsuarioId", "SubcategoriaId");
                    j.ToTable("UsuarioSubcategorias");
                });

        base.OnModelCreating(modelBuilder);
    }
}
