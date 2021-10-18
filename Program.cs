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

            InserirDadosEmMassa();
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
            // Eh recomendado escolher uma das 2 primeiras opcoes abaixo:

            //db.Produtos.Add(produto);         //Deu pau, olhar depois.
            db.Set<Produto>().Add(produto);     // Dentro de Set informamos qual entidade desejamos interagir e em Add, a instancia
            //db.Entry(produto).State = EntityState.Added;
            //db.Add(produto);                  // Nao escalavel, a aplicacao tem que descobrir o tipo do dade da variavel produto.

            // Ate entao as alteracoes nao foram para o banco de dados,
            // eh necessario informar ao EF Core que desejamos salvar as alteracoes.

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}
