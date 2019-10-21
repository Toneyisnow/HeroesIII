using H3Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{

    /// <summary>
    /// A creature object on the Adventure map
    /// </summary>
    public class CGCreature : CGObject
    {
        public CGCreature()
        {

        }

        public CreatureSet Creatures
        {
            get; set;
        }

    }
}
