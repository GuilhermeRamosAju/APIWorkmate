using APIWorkmate.Models;
using Microsoft.EntityFrameworkCore;

namespace APIWorkmate.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Contratacao> Contratacoes { get; set; }
    public DbSet<Mensagem> Mensagens { get; set; }
    public DbSet<Servico> Servicios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

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
    }
}
