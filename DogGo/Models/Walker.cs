namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int NeighborhoodId { get; set; }
        public string ImageUrl { get; set; }

        //created an instance of a new neighborhood for creating a new Walker.
        public Neighborhood Neighborhood { get; set; } 

    }
}
