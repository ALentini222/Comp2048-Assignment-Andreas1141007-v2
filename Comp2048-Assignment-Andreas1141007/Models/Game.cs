using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comp2048_Assignment_Andreas1141007.Models
{
    public class Game
    {
        public int GameId { get; set; }
        [Required]
        [MaxLength(100)]
        public string GameName { get; set; }
        [Required]
        [MaxLength(500)]
        public double AverageRating { get; set; }
        public double AveragePlaytime { get; set; }

    }
}
