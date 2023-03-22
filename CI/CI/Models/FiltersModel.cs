


using CI_Entity.Models;

namespace CI.Models
   
{
    public class FiltersModel
    {
         

       
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Mission> Missions { get; set; }


    }
}
