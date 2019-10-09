﻿using H3Engine.Components.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.Mapping
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

        public List<H3Object> Objects
        {
            get; set;
        }

        public List<ObjectTemplate> ObjectTemplates
        {
            get; set;
        }
    }
}