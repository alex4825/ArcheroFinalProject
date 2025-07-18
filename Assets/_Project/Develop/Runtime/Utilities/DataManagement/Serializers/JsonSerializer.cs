using Newtonsoft.Json;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.Serializers
{
    public class JsonSerializer : IDataSerializer
    {
        public TData Deserialize<TData>(string serializedDta)
        {
            return JsonConvert.DeserializeObject<TData>(serializedDta);
        }

        public string Serialize<TData>(TData data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
