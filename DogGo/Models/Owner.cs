using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Owner
    {

        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hmm....you should really add a name...")]
        [MaxLength(35)]
        public string Name { get; set; }

        public string Address { get; set; }
        [Required]
        [DisplayName("Neighborhood")]
        public Int32 NeighborhoodId   { get; set; }

        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        public List<Dog> DogList { get; set; } 
    }
}
