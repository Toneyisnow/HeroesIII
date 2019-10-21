using H3Engine.Common;
using H3Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components
{
    /// <summary>
    /// Contains simple two values: CreatureId, Amount
    /// </summary>
    public class StackDescriptor
    {
        public StackDescriptor()
        {

        }

        public H3Creature Creature
        {
            get; set;
        }

        public int Amount
        {
            get; set;
        }
    }


    /// <summary>
    /// Maxium of 7 stacks 
    /// </summary>
    public class CreatureSet
    {

        public EArmyFormationType FormationType
        {
            get; set;
        }

        public List<StackDescriptor> Stacks
        {
            get; set;
        }

    }
}
