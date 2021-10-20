## Tips

### Migrations:

    - Realizar migrations:
        db.Database.Migrate();

    - Verifica se existe migracoes pendentes
        var existe = db.Database.GetPendingMigrations().Any();
        if(existe)
        {
            // TODO
        }


### Comando para Aplicar Migrações:

    dotnet ef database update -p CursoEFCore.csproj -v


### Post:

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

            - Metodo 1:
            //db.Produtos.Add(produto);         
            // Deu pau, olhar depois.

            - Metodo 2:
            db.Set<Produto>().Add(produto);     
            // Dentro de Set informamos qual entidade desejamos interagir e em Add, a instancia

            - Metodo 3:
            //db.Entry(produto).State = EntityState.Added;

            - Metodo 4:
            //db.Add(produto);                  
            // Nao escalavel, a aplicacao tem que descobrir o tipo do dade da variavel produto.

            // Ate entao as alteracoes nao foram para o banco de dados,
            // eh necessario informar ao EF Core que desejamos salvar as alteracoes.

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

### Get:

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

                - Metodo 1:
                db.Clientes.Find(cliente.Id);
                // Primeiro consulta os objetos em memoria, dps na base de dados.

                - Metodo 2:
                db.Clientes.FirstOrDefault(p=>p.Id==cliente.Id);
            }
        }

## Update

    - Atualizar Dados Modificando Apenas A Propriedade Alterada:

    private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            // Solicita que a aplicacao encontre a entidade com a PK = 1

            cliente.Nome = "Cliente Alterado Passo 1";

            db.SaveChanges();
            // Metodo que deve ser chamado sempre que seja necessario salvar as alteracoes na Base de Dados.
        }


    - Atualizar Dados Modificando Todas As Propriedades, Modo 1:

    private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            // Solicita que a aplicacao encontre a entidade com a PK = 1

            cliente.Nome = "Cliente Alterado Passo 1";

            db.Clientes.Update(cliente);        
            // Atualiza todos as propriedades da instancia cliente, não soh oq foi alterado.
            // Sem esse comando, eh atualizado apenas a propriedade que foi alterada.

            db.SaveChanges();
            // Metodo que deve ser chamado sempre que seja necessario salvar as alteracoes na Base de Dados.
        }


    - Atualizar Dados Modificando Todas As Propriedades, Modo 2:

    private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            // Solicita que a aplicacao encontre a entidade com a PK = 1

            cliente.Nome = "Cliente Alterado Passo 1";

            db.Entry(cliente).State = EntityState.Modified;
            // Informa de maneira explicita para o EF Core que o objeto foi modificado.
            // Tambem ordena que todas as propriedades da instancia cliente devem ser atualizados.

            db.SaveChanges();
            // Metodo que deve ser chamado sempre que seja necessario salvar as alteracoes na Base de Dados.
        }

    private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            // Solicita que a aplicacao encontre a entidade com a PK = 1

            cliente.Nome = "Cliente Alterado Passo 1";

            db.Clientes.Update(cliente);        
            // Atualiza todos as propriedades da instancia cliente, não soh oq foi alterado.
            // Sem esse comando, eh atualizado apenas a propriedade que foi alterada.

            db.Entry(cliente).State = EntityState.Modified;
            // Informa de maneira explicita para o EF Core que o objeto foi modificado.
            // Tambem ordena que todas as propriedades da instancia cliente devem ser atualizados.

            db.SaveChanges();
            // Metodo que deve ser chamado sempre que seja necessario salvar as alteracoes na Base de Dados.
        }

    private static void AtualizarDadosEmCenarioDesconectado()
        {
            using var db = new Data.ApplicationContext();
            
            // Cenario Desconectado, quando os dados nao estao instanciados ainda:
            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new   
            {
                Nome = "Cliente Desconectado Passo 3",
                Telefone = "7966669999"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.SaveChanges();
        }

## Delete

    private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(2);
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

    private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(2);
            db.Clientes.Remove(cliente);
            
            db.SaveChanges();
        }

    private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(2);
            db.Remove(cliente);

            db.SaveChanges();
        }

    private static void RemoverRegistroDesconectado()
        {
            using var db = new Data.ApplicationContext();
            
            var cliente = new Cliente { Id = 3 };
            
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }