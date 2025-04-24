using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class StorageService
    {
        public static List<Audio> Audios { get; set; } = [];
        public static List<Audio> SelectedAudios { get; set; } = [];
        public static List<Sound> SelectedSounds { get; set; } = [];
        public static Dictionary<Audio, string> audioColors { get; set; } = [];
    }
}