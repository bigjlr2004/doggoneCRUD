using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        //created an instance of a new neighborhood for creating a new Walker.
        public Neighborhood Neighborhood { get; set; } 

    }
}
