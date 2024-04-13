
public class RemoveMatchingBegin : IFileManipulationDecorator
{
    private readonly string matchingString;

    public RemoveMatchingBegin(string matchingString)
    {
        this.matchingString = matchingString;
    }

    public string Manipulate(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        var fileName = Path.GetFileName(filePath);

        if (fileName.StartsWith(matchingString))
        {
            //Remove the matching string from the beginning of the filename
            fileName = fileName.Substring(matchingString.Length);
        }

        return Path.Combine(directoryPath!, fileName);
    }
}
