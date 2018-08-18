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
        private static Dictionary<string, string> _filtersClickedProductName;

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
                    _filtersRequired.Add(2, new List<string>()
                    {
                        "Brand",
                        "Model",
                        "Line",
                        "PhysicalCores",
                        "Litography"
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
                        { "Line", "Line" },
                        { "Model", "Models" },
                        { "Vram" , "VRAM Size" },
                        { "BusBandwith", "Bus Bandwith" },
                        { "PhysicalCores", "Physical Cores" },
                        { "Litography", "Litography (nm)" }
                    };
                    return _filtersDisplayName;
                }
                else
                {
                    return _filtersDisplayName;
                }
            }
        }
        public static Dictionary<string, string> FiltersClickedProductName
        {
            get
            {
                if (_filtersClickedProductName == null)
                {
                    _filtersClickedProductName = new Dictionary<string, string>()
                    {
                        { "Brand", "Brand" },
                        { "Line", "Line" },
                        { "Model", "Model" },
                        { "Vram" , "VRAM Size" },
                        { "BusBandwith", "Bus Bandwith" },
                        { "CoreBaseClockMhz", "Core Base (MHz)" },
                        { "CoreBoostClockMhz", "Core Boost (MHZ)" },
                        { "PhysicalCores", "Physical Cores" },
                        { "Threads", "Threads" },
                        { "Litography", "Litography (nm)" }
                    };
                    return _filtersClickedProductName;
                }
                else
                {
                    return _filtersClickedProductName;
                }
            }
        }




    }
}
