using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BidProjectsManager.Model.Enums
{
    public enum ProjectType
    {
        [Display(Name = "Tender Offer")]
        TenderOffer = 1,
        [Display(Name = "Acquisition")]
        Acquisition = 2
    }
}
