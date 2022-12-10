using System.Collections.Generic;

namespace WebApplication.Models.Settings
{
    public class CurrensyTypes
    {
        public CurrensyTypes() 
        {
            //CurrensyList = new List<string>();
        }
        public IReadOnlyList<string> CurrensyList { get; init; }
    }
}
