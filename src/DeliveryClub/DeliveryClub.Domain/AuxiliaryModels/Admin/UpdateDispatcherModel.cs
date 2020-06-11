using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class UpdateDispatcherModel
    {
        public string OldEmail { get; set; }
       
        public string NewEmail { get; set; }
        
        public string Password { get; set; }
    }
}
