using PresentationEditor.Server.Models;
using YDotNet.Document;

namespace PresentationEditor.Server.ViewModel
{
    public class PresentationPreviewModel
    {
        public Doc Snapshot {  get; set; }


        public PresentationPreviewModel(PresentationRoom presentationRoom)
        {
            Snapshot = presentationRoom.Presentation.Data;
        }
    }
}
