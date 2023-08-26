using Server.DomainFeatures.CounselingRessourceAggregate.Domain;
using Shared.CounselingRessource;

namespace Server.DomainFeatures.CounsellingAggregate.Domain
{
    public class CounselingRessource
    {
        public string Text { get; set; }
        public CounselingDomainType DomainType { get; set; }

        public CounselingRessourceDTO ToDTO()
        {
            return new CounselingRessourceDTO()
            {
                DomainType = (CounselingDomainTypeDTO)this.DomainType,
                Text = this.Text
            };
        }
    }
}
