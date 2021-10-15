using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEFCore.Domain
{
    //[Table("Cliente")]            Data Annotation para atribuir esta classe a uma tabela no nosso banco de dados
    public class Cliente
    {
        //[Key] 
        public int Id { get; set; }

        //[Column]
        public string Nome { get; set; }

        //[Column("Phone")]
        public string Telefone { get; set; }

        //[Column]
        public string CEP { get; set; }

        //[Column]
        public string Estado { get; set; }

        //[Column]
        public string Cidade { get; set; }
        
        public string Email { get; set; }
    }
}