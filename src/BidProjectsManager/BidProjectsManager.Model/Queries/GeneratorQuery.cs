using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Model.Queries
{
    public class GeneratorQuery
    {
        public int Reportid { get; set; }
        public int? MaxRows { get; set; }
        public IEnumerable<GeneratorParamValue> ParamsValues { get; set; }
    }
}
