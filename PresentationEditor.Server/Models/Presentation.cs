
using YDotNet.Document;

namespace PresentationEditor.Server.Models
{
    public class Presentation
    {
        public PresentationID Id { get; set; }
        public string Name { get; set; }
        public Doc Data { get; set; }
        public UserID Administrator { get; set; }
        public HashSet<UserID> Editors { get; set; }

        public Presentation(UserID creator, string name)
        {
            Id = PresentationID.NewGuid();
            Name = name;
            Data = new Doc();
            Administrator = creator;
            Editors = new HashSet<UserID>();
        }
    }
}
