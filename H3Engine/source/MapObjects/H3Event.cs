﻿using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class H3Event : H3Object
    {
        public H3Event()
        {
            this.Abilities = new List<ESecondarySkill>();
        }

        public List<ESecondarySkill> Abilities
        {
            get; set;
        }


    }
}
