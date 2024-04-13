public class ChangeExtension : IFileManipulationDecorator
{
    private readonly string newExtension;

    public ChangeExtension(string newExtension)
    {
        this.newExtension = newExtension;
    }

    public string Manipulate(string filePath)
    {
        return Path.ChangeExtension(filePath, newExtension);
    }
}