using VecEditor.IO;
using Xunit;

namespace VecEditor.IO.Tests;

public class JsonProjectSerializerTests
{
    [Fact]
    public void Serialize_EmptyDocument_ReturnsJson()
    {
        var serializer = new JsonProjectSerializer();
        var document = new ProjectDocument();

        var json = serializer.Serialize(document);

        Assert.False(string.IsNullOrWhiteSpace(json));
        Assert.Contains("Figures", json);
    }

    [Fact]
    public void Deserialize_EmptyFigures_ReturnsDocument()
    {
        var serializer = new JsonProjectSerializer();

        var document = serializer.Deserialize("{\"Figures\":[]}");

        Assert.NotNull(document);
        Assert.NotNull(document.Figures);
        Assert.Empty(document.Figures);
    }
}