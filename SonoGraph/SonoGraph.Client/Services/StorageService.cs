using SonoGraph.Client.Models;
using SonoGraph.Client.Pages;

namespace SonoGraph.Client.Services
{
    public class StorageService
    {
        public List<Audio> Audios { get; set; } = [];
        public List<Audio> SelectedAudios { get; set; } = [];
        public List<Sound> SelectedSounds { get; set; } = [];
        public Dictionary<Audio, string> audioColors { get; set; } = [];
        public AudioEditor AudioEditor { get; set; } = new AudioEditor();
    }
}