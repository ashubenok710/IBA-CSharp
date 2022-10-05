using System.ComponentModel.DataAnnotations;

namespace CinemaPortal.Web.Models
{
    public partial class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ProductionDate { get; set; } = null!;
        public string Raiting { get; set; } = null!;
        public int DirectorId { get; set; }

        public Person Director;
    }
}
