using System.ComponentModel.DataAnnotations;
using MvcSaed.Attributes;

namespace MvcSaed.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
    }
    public enum StatusTurma
    {
        Ativa = 1,
        Inativa = 0
    }

    // Removida a defini��o duplicada da classe Modalidade deste arquivo.
    // Mantenha UMA defini��o de Modalidade em outro arquivo (por exemplo Models\Modalidade.cs).
}
