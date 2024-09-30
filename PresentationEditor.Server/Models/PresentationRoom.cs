using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using YDotNet.Document;

namespace PresentationEditor.Server.Models
{
    public class PresentationRoom
    {
        public Presentation Presentation { get; set; }
        public HashSet<User> ConnectedParticipants { get; set; }


        public PresentationRoom(UserID creatorId, string name)
        {
            Presentation = new Presentation(creatorId, name);
            ConnectedParticipants = new HashSet<User>();
        }
    }
}
