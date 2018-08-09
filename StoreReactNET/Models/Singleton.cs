using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models
{
    public sealed class Singleton
    {
        private static Dictionary<int, List<string>> _filtersRequired;
        private static Dictionary<string, string> _filtersDisplayName;
        private static Dictionary<string, string> _filtersJSONName;

        public static Dictionary<int, List<string>> FiltersRequired
        {
            get
            {
                if(_filtersRequired == null)
                {
                    _filtersRequired = new Dictionary<int, List<string>>();
                    
                    _filtersRequired.Add(1, new List<string>()
                    {
                        "Brand",
                        "Model",
                        "Vram",
                        "BusBandwith"
                    });
                    return _filtersRequired;
                }
                else
                {
                    return _filtersRequired;
                }
            }
        }
        public static Dictionary<string, string> FiltersDisplayName
        {
            get
            {
                if(_filtersDisplayName == null)
                {
                    _filtersDisplayName = new Dictionary<string, string>()
                    {
                        { "Brand", "Brands" },
                        { "Model", "Models" },
                        { "Vram" , "VRAM Size" },
                        { "BusBandwith", "Bus Bandwith" }
                    };
                    return _filtersDisplayName;
                }
                else
                {
                    return _filtersDisplayName;
                }
            }
        }
        public static Dictionary<string, string> FiltersJSONName
        {
            get
            {
                if(_filtersJSONName == null)
                {
                    _filtersJSONName = new Dictionary<string, string>()
                    {
                        { "Brand", "brands" },
                        { "Model", "models" },
                        { "Vram", "vramList" },
                        { "BusBandwith", "busWithList" }
                    };
                    return _filtersJSONName;

                }
                else
                {
                    return _filtersJSONName;
                }
            }
        }



    }
}
