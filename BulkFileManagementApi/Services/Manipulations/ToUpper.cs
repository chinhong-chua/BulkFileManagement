public class ToUpper : IFileManipulationDecorator
{
    public string Manipulate(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        var extension = Path.GetExtension(filePath);
        var upperFileName = $"{fileNameWithoutExtension.ToUpper()}{extension.ToUpper()}";
        return Path.Combine(directoryPath!, upperFileName);
    }
}