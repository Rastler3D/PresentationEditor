using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationEditor.Server.Models;
using PresentationEditor.Server.Services;
using PresentationEditor.Server.ViewModel;

namespace PresentationEditor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PresentationController : ControllerBase
    {
        readonly PresentationService presentationService;

        public PresentationController(PresentationService presentationService) {
            this.presentationService = presentationService;
        }

        [HttpGet]
        public ActionResult<PresentationInfoModel> GetPresentations()
        {
            var presentations = presentationService.GetPresentations().Select(x => new PresentationInfoModel(x));

            return Ok(presentations);
        }

        [HttpGet("{presentationId}/preview")]
        public ActionResult<PresentationPreviewModel> GetPresentationPreview(Guid presentationId)
        {
            var presentation = presentationService.GetPresentation(presentationId);

            if (presentation != null) {
                return Ok(new PresentationPreviewModel(presentation));
            }

            return NotFound();
        }


        [HttpPost]
        public ActionResult<PresentationRoom> CreatePresentation([FromQuery] string name) {
            var user = new User(HttpContext.User);

            return Ok(presentationService.CreatePresentation(user.Id, name));
        }

        [HttpDelete("{presentationId}")]
        public async Task<ActionResult> RemovePresentation(Guid presentationId)
        {
            var user = new User(HttpContext.User);
            await presentationService.RemovePresentation(user.Id, presentationId);

            return NoContent();
        }

        [HttpPost("{presentationId}/editor/{editorId}")]
        public async Task<ActionResult> AddEditor(Guid presentationId, Guid editorId)
        {
            var user = new User(HttpContext.User);
            await presentationService.AddEditor(user.Id, presentationId, editorId);

            return Created();
        }

        [HttpDelete("{presentationId}/editor/{editorId}")]
        public async Task<ActionResult> RemoveEditor(Guid presentationId, Guid editorId)
        {
            var user = new User(HttpContext.User);
            await presentationService.RemoveEditor(user.Id, presentationId, editorId);

            return NoContent();
        }
    }
}
