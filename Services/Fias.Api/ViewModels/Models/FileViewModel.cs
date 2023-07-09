using Fias.Api.Models.File;

namespace Fias.Api.ViewModels.Models
{
    public class FileViewModel
    {
        public FileViewModel() 
        {
            TempFiles = new List<TempFile>();
            SelectedRegions = new List<string>();
        }
        public List<string> SelectedRegions { get; set; }
        public List<TempFile> TempFiles { get; set; }
    }
}
