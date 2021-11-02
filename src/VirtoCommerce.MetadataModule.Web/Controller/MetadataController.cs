using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.MetadataModule.Core.Model;
using VirtoCommerce.MetadataModule.Core.Services;

namespace VirtoCommerce.MetadataModule.Web.Controllers.Api
{
    [Route("api/metadata")]
    public class MetadataController : Controller
    {
        private readonly IMetadataService _metadataService;

        public MetadataController(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        [HttpPost]
        [Route("{typeName}/{projectionName}")]
        public ActionResult<Projection> GetProjection([FromRoute] string typeName, [FromRoute] string projectionName)
        {
            var result = _metadataService.GetProjection(_metadataService.GetModels().FirstOrDefault(x => x.Name == typeName), projectionName);
            return Ok(result);
        }
    }
}
