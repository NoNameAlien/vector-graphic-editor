namespace VecEditor.IO.Project;

public interface IProjectSerializer
{
    string Serialize(object project);
    T Deserialize<T>(string json);
}