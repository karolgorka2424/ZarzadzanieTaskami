using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarzadzanieTaskami.Models
{
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Opis { get; set; }

        public bool CzyZakonczony { get; set; }

        // Klucz obcy dla Projektu
        public int ProjektId { get; set; }
        public Projekt? Projekt { get; set; }

        // Relacja jeden-do-wielu z Komentarz
        public List<Komentarz>? Komentarze { get; set; }
    }
}
