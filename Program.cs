using System;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            // db.Database.Migrate();

            // Verifica se existe migracoes pendentes
            var existe = db.Database.GetPendingMigrations().Any();
            if(existe)
            {
                // TODO
            }

            //Console.WriteLine("Hello World!");

            //InserirDados();
            //InserirDadosEmMassa();
            ConsultarDados();

        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            // Metodo 1:
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();

            // Metodo 2: (Recomendado)
            var consultaPorMetodo = db.Clientes.AsNoTracking() 
                // AsNoTracking() faz com que o EF Core nao rastreie os obj em memoria. 
                // Forca a consultar direto na Base de Dados.
                .Where(p=>p.Id >0)
                .OrderBy(p=>p.Id)
                .ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");

                //db.Clientes.Find(cliente.Id);
                // Primeiro consulta os objetos em memoria, dps na base de dados.

                db.Clientes.FirstOrDefault(p=>p.Id==cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Rafaela Almeida",
                CEP = "31150000",
                Cidade = "Belo Horizonte",
                Estado = "MG",
                Telefone = "319999999"
            };

            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            // Eh recomendado escolher o metodo 1 ou 2:

            // Metodo 1:
            //db.Produtos.Add(produto);         
            // Deu pau, olhar depois.

            // Metodo 2:
            db.Set<Produto>().Add(produto);     
            // Dentro de Set informamos qual entidade desejamos interagir e em Add, a instancia

            // Metodo 3:
            //db.Entry(produto).State = EntityState.Added;

            // Metodo 4:
            //db.Add(produto);                  
            // Nao escalavel, a aplicacao tem que descobrir o tipo do dade da variavel produto.

            // Ate entao as alteracoes nao foram para o banco de dados,
            // eh necessario informar ao EF Core que desejamos salvar as alteracoes.

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}