using System.Collections.Generic;
using System.Linq;

namespace StoreReactNET.Services.Product.Models.Inputs
{
    public class JSONCategoryFilters
    {
        public List<JSONCategoryFilter> Filters;

        public JSONCategoryFilters()
        {
            this.Filters = new List<JSONCategoryFilter>();
        }
        public JSONCategoryFilter GetFilter(string _Type)
        {
            foreach(var entry in this.Filters)
            {
                if(entry.Type == _Type)
                {
                    return entry;
                }
            }
            return null;
        }
        public void DistinctFilters()
        {
            foreach (var filter in this.Filters)
            {
                filter.Variables = filter.Variables.Distinct().ToList();
            }
        }
    }
    public class JSONCategoryFilter
    {
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public List<string> Variables { get; set; }

        public JSONCategoryFilter(string _Type)
        {
            this.Type = _Type;
            this.DisplayName = Singleton.FiltersDisplayName[_Type];
            this.Variables = new List<string>();
        }
    }
}
