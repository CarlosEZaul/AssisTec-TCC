using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssisTec.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


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
                    mysqlOptions.ServerVersion(new Version(10, 4, 32), ServerType.MariaDb)
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
    
                entity.Property(e => e.Nome).HasColumnName("nome");
                entity.Property(e => e.Cpf).HasColumnName("cpf");
                entity.Property(e => e.Senha).HasColumnName("senha");
                entity.Property(e => e.Telefone).HasColumnName("telefone");
                entity.Property(e => e.Nivel).HasColumnName("nivel");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Cep).HasColumnName("cep");
                entity.Property(e => e.Rua).HasColumnName("rua");
                entity.Property(e => e.Numero).HasColumnName("numero");
                entity.Property(e => e.Cidade).HasColumnName("cidade");
                entity.Property(e => e.Bairro).HasColumnName("bairro");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Complemento).HasColumnName("complemento");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");
    
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_cliente");
    
                entity.Property(e => e.Nome).HasColumnName("nome");
                entity.Property(e => e.Cpf).HasColumnName("cpf");
                entity.Property(e => e.Telefone).HasColumnName("telefone");
                entity.Property(e => e.DataNascimento).HasColumnName("datanasc");
                entity.Property(e => e.Cep).HasColumnName("cep");
                entity.Property(e => e.Rua).HasColumnName("rua");
                entity.Property(e => e.Numero).HasColumnName("numero");
                entity.Property(e => e.Cidade).HasColumnName("cidade");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Bairro).HasColumnName("bairro");
                entity.Property(e => e.Complemento).HasColumnName("complemento");
            });

            modelBuilder.Entity<ContasReceber>(entity =>
            {
                entity.ToTable("contas_receber");

                entity.HasKey(e => e.id_conta_receber);
                entity.Property(e => e.id_conta_receber).HasColumnName("id_conta_receber");
    
                entity.Property(e => e.descricao).HasColumnName("descricao");
                entity.Property(e => e.valor).HasColumnName("valor");
                modelBuilder.Entity<ContasReceber>()
                    .Property(p => p.valor).HasMaxLength(18);
                entity.Property(e => e.data_emissao).HasColumnName("data_emissao");
                entity.Property(e => e.data_pagamento).HasColumnName("data_pagamento");
                entity.Property(e => e.data_vencimento).HasColumnName("data_vencimento");
                entity.Property(e => e.status).HasColumnName("status");
                entity.Property(e => e.observacoes).HasColumnName("observacoes");

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
    
                entity.Property(e => e.Descricao).HasColumnName("descricao");
                entity.Property(e => e.Marca).HasColumnName("marca");
                entity.Property(e => e.Modelo).HasColumnName("modelo");
                entity.Property(e => e.Numero_Serie).HasColumnName("numero_serie");
                entity.Property(e => e.estado_entrada).HasColumnName("estado_entrada");
                entity.Property(e => e.acessorios).HasColumnName("acessorios");
    
                entity.Property(e => e.Observacoes)
                    .HasColumnName("observacoes")
                    .HasColumnType("text");
            });
            
            modelBuilder.Entity<OrdemServico>(entity =>
            {
                entity.ToTable("ordem_servico");
                entity.HasKey(e => e.id_os);
                entity.Property(e => e.id_os).HasColumnName("id_os");

                entity.Property(e => e.status).HasColumnName("status");
                entity.Property(e => e.data_abertura).HasColumnName("data_abertura");
                entity.Property(e => e.data_atualizacao).HasColumnName("data_atualizacao");
                entity.Property(e => e.data_fechamento).HasColumnName("data_fechamento");
    
                entity.Property(e => e.valor_mao_obra).HasColumnName("valor_mao_obra");
                entity.Property(e => e.valor_pecas).HasColumnName("valor_pecas");
                entity.Property(e => e.valor_total).HasColumnName("valor_total");
    
                entity.Property(e => e.problema_relatado).HasColumnName("problema_relatado");
                entity.Property(e => e.diagnostico).HasColumnName("diagnostico");
                entity.Property(e => e.observacoes).HasColumnName("observacoes");

                entity.HasOne(e => e.Tecnico)
                    .WithMany()
                    .HasForeignKey(e => e.id_tecnico)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Cliente)
                    .WithMany()
                    .HasForeignKey(e => e.id_cliente)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Equipamento)
                    .WithMany()
                    .HasForeignKey(e => e.id_equipamento)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("forma_pagamento");

                entity.HasKey(e => e.Idforma_pagamento);
                entity.Property(e => e.Idforma_pagamento).HasColumnName("id_forma_pagamento");
                entity.Property(e => e.Descricao).HasColumnName("descricao");

                entity.HasData(
                    new Pagamento{ Idforma_pagamento = 1, Descricao = "---"},
                    new Pagamento { Idforma_pagamento = 2, Descricao = "Pix" },
                    new Pagamento { Idforma_pagamento = 3, Descricao = "Cartão de Crédito / Débito" },
                    new Pagamento { Idforma_pagamento = 4, Descricao = "Dinheiro" }
                );
            });

        }
        
    }
}