public class AddPrefix : IFileManipulationDecorator
{
    private readonly string prefix;

    public AddPrefix(string prefix)
    {
        this.prefix = prefix;
    }

    public string Manipulate(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        var fileName = Path.GetFileName(filePath);
        return Path.Combine(directoryPath!, prefix + fileName);
    }
}