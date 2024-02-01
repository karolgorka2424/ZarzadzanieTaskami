using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieTaskami.Models
{
    public class Komentarz
    {
        public int KomentarzId { get; set; }

        [Required]
        public string Tresc { get; set; }

        // Klucz obcy dla Task
        public int TaskId { get; set; }
        public ProjectTask? Task { get; set; }
    }
}
