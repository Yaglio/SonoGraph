using SonoGraph.Client.Models;

namespace SonoGraph.Client.Services
{
    public class StorageService
    {
        public static List<Audio> Audios { get; set; } = [];
        public static List<Audio> selected { get; set; } = [];
        public static Dictionary<Audio, string> audioColors { get; set; } = [];
    }
}