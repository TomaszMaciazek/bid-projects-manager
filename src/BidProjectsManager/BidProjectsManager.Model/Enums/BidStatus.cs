using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BidProjectsManager.Model.Enums
{
    public enum BidStatus
    {
        [Display(Name = "Bid Preparation")]
        BidPreparation = 1,
        [Display(Name = "Won")]
        Won = 2,
        [Display(Name = "Lost")]
        Lost = 3,
        [Display(Name = "No Bid")]
        NoBid = 4,
        [Display(Name = "Awaiting Signature")]
        AwaitingSignature = 5
    }
}
