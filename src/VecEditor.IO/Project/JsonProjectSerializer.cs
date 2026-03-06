using Newtonsoft.Json;

namespace VecEditor.IO.Project;

public class JsonProjectSerializer : IProjectSerializer
{
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };

    public string Serialize(object project)
        => JsonConvert.SerializeObject(project, _settings);

    public T Deserialize<T>(string json)
        => JsonConvert.DeserializeObject<T>(json, _settings)!;
}