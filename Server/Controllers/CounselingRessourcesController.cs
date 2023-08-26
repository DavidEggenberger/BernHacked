using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DomainFeatures.ChatAggregate.Infrastructure;
using Server.DomainFeatures.CounselingRessourceAggregate.Infrastructure;
using Shared.Chat;
using Shared.CounselingRessource;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounselingRessourcesController : ControllerBase
    {
        private readonly CounselingRessourcePersistence counselingRessourcePersistence;
        public CounselingRessourcesController(CounselingRessourcePersistence counselingRessourcePersistence)
        {
            this.counselingRessourcePersistence = counselingRessourcePersistence;
        }

        /// <summary>
        /// Retrieve all counseling ressources
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<CounselingRessourceDTO>> GetCounselingRessources()
        {
            return Ok(counselingRessourcePersistence.CounselingRessources.Select(x => x.ToDTO()));
        }
    }
}
