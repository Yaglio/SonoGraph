using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class StorageService
    {
        public List<Audio> Audios { get; set; } = new ();
        public List<Audio> SelectedAudios { get; set; } = new ();
        public List<Sound> SelectedSounds { get; set; } = new ();
        public Dictionary<Audio, string> audioColors { get; set; } = new ();
    }
}