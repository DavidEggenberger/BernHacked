using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.CounselingRessource
{
    public class CounselingRessourceDTO
    {
        public string Text { get; set; }
        public CounselingDomainTypeDTO DomainType { get; set; }
    }
}
