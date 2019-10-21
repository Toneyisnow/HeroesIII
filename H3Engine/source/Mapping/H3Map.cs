using H3Engine.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Mapping
{
    public class H3Map
    {
        
        public H3Map()
        {
            this.Header = new MapHeader();
        }

        public MapHeader Header
        {
            get; private set;
        }

        public List<CGObject> Objects
        {
            get; set;
        }

        public List<ObjectTemplate> ObjectTemplates
        {
            get; set;
        }

        public CGGrail Grail
        {
            get; set;
        }
    }
}
