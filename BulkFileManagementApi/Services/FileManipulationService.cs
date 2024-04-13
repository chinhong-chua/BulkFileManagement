using BulkFileManagementApi.Models;

namespace BulkFileManagementApi.Services
{
    public class FileManipulationService : IFileManipulationService
    {
        public async Task<IEnumerable<string>> ManipulateFilesAsync(FileManipulationRequest request)
        {
            //Retrieve list of files 
            var filesToProcess = new List<string>();
            var manipulatedFiles = new List<string>();
            var errors = new List<string>(); //Keeping track of errors occured

            // Handle files declared
            if (request.Files != null && request.Files.Any())
            {
                foreach (var file in request.Files)
                {
                    Console.WriteLine($"{file} has been processed.");
                    filesToProcess.Add(Path.Combine(request.FolderPath, file)); //Combine folder path with file name
                }
            }
            else if (!string.IsNullOrWhiteSpace(request.FolderPath))
            {
                //Process files from the specified folder path
                SearchOption searchOption = request.IncludeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                filesToProcess.AddRange(Directory.GetFiles(request.FolderPath, "*.*", searchOption));
            }

            foreach (var file in filesToProcess)
            {
                try
                {
                    //Apply desired manipulation for each file
                    var manipulatedFilePath = ApplyManipulation(file, request.Manipulations);
                    //Rename the file
                    File.Move(file, manipulatedFilePath);
                    manipulatedFiles.Add(manipulatedFilePath);
                }
                catch (Exception ex)
                {
                    errors.Add($"Error on {file}: {ex.Message}");
                }
            }
            if (errors.Count() > 0)
            {
                return errors;
            }

            return manipulatedFiles;
        }

        private string ApplyManipulation(string filePath, List<string> manipulations)
        {
            var manipulatedFilePath = filePath;

            foreach (var manipulation in manipulations)
            {
                var parts = manipulation.Split(':');
                //Convert to enum type
                var manipulationType = (FileManipulationType)Enum.Parse(typeof(FileManipulationType), parts[0]);
                var param1 = parts.Length > 1 ? parts[1] : null;
                var param2 = parts.Length > 2 ? parts[2] : null;

                IFileManipulationDecorator decorator = null;

                switch (manipulationType)
                {
                    case FileManipulationType.ChangeExtension:
                        if (param1 != null)
                            decorator = new ChangeExtension(param1);
                        break;
                    case FileManipulationType.AddPrefix:
                        if (param1 != null)
                            decorator = new AddPrefix(param1);
                        break;
                    case FileManipulationType.ToUpper:
                        decorator = new ToUpper();
                        break;
                    case FileManipulationType.RemoveMatchingBegin:
                        decorator = new RemoveMatchingBegin(param1!);
                        break;
                    case FileManipulationType.ReplaceText:
                        decorator = new ReplaceText(param1!, param2!);
                        break;
                    default:
                        continue;
                }
                if (decorator != null)
                {
                    manipulatedFilePath = decorator.Manipulate(manipulatedFilePath);
                }
            }

            return manipulatedFilePath;
        }

    }
}
