using System.Collections.Generic;

namespace DealersAndVehicles.Models
{
    public class DealerAnswer
    {
        public int dealerId { get; set; }
        public string name { get; set; }
        public List<VehicleAnswer> vehicles { get; set; }
    }
}
