using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarzadzanieTaskami.Models
{
    public class Projekt
    {
        public int ProjektId { get; set; }

        [Required]
        public string Nazwa { get; set; }

        // Relacja jeden-do-wielu z Task
        public List<ProjectTask>? Tasks { get; set; }
    }
}
