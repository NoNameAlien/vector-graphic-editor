using Newtonsoft.Json;

namespace VecEditor.IO;

public sealed class JsonProjectSerializer
{
    public string Serialize(ProjectDocument document)
    {
        return JsonConvert.SerializeObject(document, Formatting.Indented);
    }

    public ProjectDocument Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<ProjectDocument>(json) ?? new ProjectDocument();
    }
}