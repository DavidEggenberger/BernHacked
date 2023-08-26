using Server.DomainFeatures.ChatAggregate.Domain;
using Server.DomainFeatures.CounsellingAggregate.Domain;
using System.Collections.Generic;

namespace Server.DomainFeatures.CounselingRessourceAggregate.Infrastructure
{
    public class CounselingRessourcePersistence
    {
        public IList<CounselingRessource> CounselingRessources { get; set; }
        public CounselingRessourcePersistence()
        {
            CounselingRessources = new List<CounselingRessource>()
            {
                new CounselingRessource
                {
                    Text = "",
                    DomainType = Domain.CounselingDomainType.Fear
                },
                new CounselingRessource
                {
                    Text = "",
                    DomainType = Domain.CounselingDomainType.Stress
                },
                new CounselingRessource
                {
                    Text = "",
                    DomainType = Domain.CounselingDomainType.BreathingExercises
                }
            };
        }
    }
}

