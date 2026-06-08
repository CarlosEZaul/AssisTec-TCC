using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssisTec.Models;


namespace AssisTec.Repository
{
    public class AppDbContext:DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<OrdemServico> OrdemServicos { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<ContasReceber> ContasReceber { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "SERVER=localhost; DATABASE=assistec; UID=root; PWD=; PORT=3306;";

                optionsBuilder.UseMySql(connectionString, mysqlOptions => 
                    mysqlOptions.ServerVersion("10.4.32-mariadb")
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_usuario");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_cliente");
            });

            modelBuilder.Entity<ContasReceber>(entity =>
            {
                entity.ToTable("contas_receber");
    
                entity.HasKey(e => e.id_conta_receber);
                entity.Property(e => e.id_conta_receber).HasColumnName("id_conta_receber");

                entity.HasOne(e => e.OrdemServico)
                    .WithMany()
                    .HasForeignKey(e => e.id_os_fk)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Pagamento)
                    .WithMany()
                    .HasForeignKey(e => e.id_forma_pagamento_fk)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.ToTable("equipamentos");
                entity.HasKey(e => e.Id_equipamento);
                entity.Property(e => e.Id_equipamento).HasColumnName("id_equipamento");

            });
            
            modelBuilder.Entity<OrdemServico>(entity =>
            {
                entity.ToTable("ordem_servico");
                entity.HasKey(e => e.id_os);
                entity.Property(e => e.id_os).HasColumnName("id_os");
                entity.HasOne(e => e.Tecnico)
                    .WithMany()
                    .HasForeignKey("id_tecnico")
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Cliente)
                    .WithMany()
                    .HasForeignKey("id_cliente")
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Equipamento)
                    .WithMany()
                    .HasForeignKey("id_equipamento")
                    .OnDelete(DeleteBehavior.SetNull);

            });
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("forma_pagamento");
                entity.HasKey(e => e.Idforma_pagamento);
                entity.Property(e => e.Idforma_pagamento).HasColumnName("id_forma_pagamento");
                entity.Property(e => e.Descricao).HasColumnName("descricao");
            });
            
            
            
            

        }
    }
}