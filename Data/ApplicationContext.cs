using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p=>p.AddConsole());

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {   // CLASSE RESPONSAVEL POR ESTABELECER UMA SESSÃO ENTRE A APLICAÇÃO E SEU BANCO DE DADOS
            // String de conexão:
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MODOS DE GERAR AS TABELAS A PARTIR DE CASSES EXISTENTES:

            //          MODO 1: IDENTIFICANDO AS CLASSES AUTOMATICAMENTE APARTIR DAS [...]Configurations.cs CRIADAS


            // Procura as classes completas que estao complementando o IEntityTypeConfiguration:

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);


            /* 
                        MODO 2: ESPECIFICANDO CLASSE POR CLASSE COM A CRIAÇÃO DAS classes de configuração


            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());

            */




            /*
                        MODO 3: CRIANDO ENTIDADE POR ENTIDADE DIRETAMENTE NA CLASSE ApplicationContext.cs
                                    (Sem a necessidade de criar as classes de configuração)
             

            modelBuilder.Entity<Cliente>(p =>
            {
                p.ToTable("Clientes");
                // Adicionando colunas às tabelas:
                p.HasKey(p => p.Id);
                p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired();
                p.Property(p => p.Telefone).HasColumnType("CHAR(11)");
                p.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
                p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
                p.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

                p.HasIndex(i => i.Telefone).HasDatabaseName("idx_cliente_telefone");
            });

            modelBuilder.Entity<Produto>(p =>
            {
                p.ToTable("Produtos");
                p.HasKey(p => p.Id);
                p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.TipoProduto).HasConversion<string>();
            });

            modelBuilder.Entity<Pedido>(p=>
            {
                p.ToTable("Pedidos");
                p.HasKey(p => p.Id);
                p.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                p.Property(p => p.Status).HasConversion<string>();
                p.Property(p => p.TipoFrete).HasConversion<int>();
                p.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

                p.HasMany(p => p.Itens)
                    .WithOne(p => p.Pedido)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PedidoItem>(p =>
            {
                p.ToTable("PedidoItens");
                p.HasKey(p => p.Id);
                p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired();
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.Desconto).IsRequired();
            });
            */
        }
    }
}