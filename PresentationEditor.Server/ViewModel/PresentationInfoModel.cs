using PresentationEditor.Server.Models;
using YDotNet.Document;

namespace PresentationEditor.Server.ViewModel
{
    public class PresentationInfoModel
    {
        public PresentationID Id { get; set; }
        public string Name { get; set; }

        public PresentationInfoModel(PresentationRoom room)
        {
            Id = room.Presentation.Id;
            Name = room.Presentation.Name;
        }
    } 
}
