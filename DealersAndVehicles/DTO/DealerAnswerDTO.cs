using System.Collections.Generic;

namespace DealersAndVehicles.DTO
{
    public class DealerAnswerDTO
    {
        public int dealerId { get; set; }
        public string name { get; set; }
        public List<VehicleAnswerDTO> vehicles { get; set; }
    }
}
