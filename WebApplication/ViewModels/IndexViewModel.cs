using System.Collections.Generic;
using WebApplication.Models.Settings;

namespace WebApplication.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel() 
        {
            Currensies = new List<Currensy>();
        }

        public List<Currensy> Currensies { get; init; }
    }
}
