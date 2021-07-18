using Newtonsoft.Json;

namespace SkyEditor.RomEditor.Infrastructure.Automation
{
    public class ScriptUtilities
    {
        public string JsonSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T? DeserializeJson<T>(string json) where T: class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
