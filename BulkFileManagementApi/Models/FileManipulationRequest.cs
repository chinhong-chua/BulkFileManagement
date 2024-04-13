using System.ComponentModel.DataAnnotations;

namespace BulkFileManagementApi.Models
{

    public class FileManipulationRequest
    {
        [Required(ErrorMessage = "Folder path is required.")]
        public string FolderPath { get; set; }
        public List<string> Files { get; set; }
        [Required(ErrorMessage = "IncludeSubfolders is required.")]
        public bool IncludeSubfolders { get; set; }
        [RequiredListAttribute(ErrorMessage = "Manipulations are required.")]
        public List<string> Manipulations { get; set; }
    }

    public class RequiredListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IList<string> list)
            {
                return list.Count > 0; //Check if the list has at least one item
            }
            return false;
        }
    }
}
