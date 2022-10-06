using System.ComponentModel.DataAnnotations;

namespace CinemaPortal.Web.Models
{
    public partial class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ProductionDate { get; set; } = null!;
        public int Raiting { get; set; }
        public int DirectorId { get; set; }

        public Person Director;
    }
}
