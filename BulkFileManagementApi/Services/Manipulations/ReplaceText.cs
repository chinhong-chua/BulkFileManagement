
public class ReplaceText : IFileManipulationDecorator
{
    private readonly string oldString;
    private readonly string newString;

    public ReplaceText(string oldString, string newString)
    {
        this.oldString = oldString;
        this.newString = newString;
    }

    public string Manipulate(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        var fileName = Path.GetFileName(filePath);

        //Remove the matching string
        fileName = fileName.Replace(oldString, newString);


        return Path.Combine(directoryPath!, fileName);
    }
}
