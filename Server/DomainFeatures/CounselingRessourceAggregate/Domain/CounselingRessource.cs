using Shared.CounselingRessource;

namespace Server.DomainFeatures.CounsellingAggregate.Domain
{
    public class CounselingRessource
    {
        public string Name { get; set; }
        public int MyProperty { get; set; }

        public CounselingRessourceDTO ToDTO()
        {
            return new CounselingRessourceDTO();
        }
    }
}
