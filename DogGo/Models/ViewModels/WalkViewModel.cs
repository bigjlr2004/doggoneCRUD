using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkViewModel
    {
        public Walk Walk { get; set; }
        public List<Walker> Walkers { get; set; } 
        public List<Dog> Dogs {get; set;} 
        public List<int> SelectedDogs { get; set;}
    }
}
