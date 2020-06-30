using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Domain.AuxiliaryModels.SuperUser
{
    public class UpdateDispatcherModel
    {
        public int Id { get; set; }

        public string OldEmail { get; set; }
       
        public string NewEmail { get; set; }
        
        public string Password { get; set; }
    }
}
