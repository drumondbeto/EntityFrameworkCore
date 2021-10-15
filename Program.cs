using System;
using System.Linq;
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

            Console.WriteLine("Hello World!");
        }

        private static void InserirDados()
        {
            
        }
    }
}
