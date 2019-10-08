using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3MapLoader.Components.Mapping
{
    public class HMMap
    {
        
        public HMMap()
        {
            this.Header = new MapHeader();
        }

        public MapHeader Header
        {
            get; private set;
        }


    }
}
