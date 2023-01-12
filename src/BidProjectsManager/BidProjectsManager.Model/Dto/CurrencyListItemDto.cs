using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Model.Dto
{
    public class CurrencyListItemDto : CurrencyDto
    {
        public bool IsDeletable { get; set; } = false;
    }
}
