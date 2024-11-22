using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using CHRISTIANEXAMENFINAL.Models;
using CHRISTIANEXAMENFINAL.Data;
using CHRISTIANEXAMENFINAL.Controllers;

namespace CHRISTIANEXAMENFINAL.Data
{
        public class EventContext : DbContext
        {
            public EventContext(DbContextOptions<EventContext> options) : base(options) { }

            public DbSet<TipoEvento> TiposEvento { get; set; }
            public DbSet<EventoCorporativo> EventosCorporativos { get; set; }
            public DbSet<AsistenteEvento> AsistentesEvento { get; set; }

        public class EventContextd : DbContext
        {
            public EventContextd(DbContextOptions<EventContext> options) : base(options) { }



            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<EventoCorporativo>()
                    .HasOne<TipoEvento>()
                    .WithMany()
                    .HasForeignKey(e => e.TipoEventoID)
                    .OnDelete(DeleteBehavior.Restrict);

                base.OnModelCreating(modelBuilder);
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Relación entre EventoCorporativo y TipoEvento (con clave foránea)
                modelBuilder.Entity<EventoCorporativo>()
                    .HasOne<TipoEvento>()
                    .WithMany()
                    .HasForeignKey(e => e.TipoEventoID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación entre AsistenteEvento y EventoCorporativo
                modelBuilder.Entity<AsistenteEvento>()
                    .HasOne<EventoCorporativo>()
                    .WithMany()
                    .HasForeignKey(a => a.EventoID)
                    .OnDelete(DeleteBehavior.Restrict);

                base.OnModelCreating(modelBuilder);
            // Configuración de la clave primaria para AsistenteEvento
            modelBuilder.Entity<AsistenteEvento>()
                .HasKey(ae => ae.AsistenteID);  // Definir la clave primaria
        }
        }
    }
