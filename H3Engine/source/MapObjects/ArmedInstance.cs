using H3Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class ArmedInstance : CGObject
    {
        public ArmedInstance()
        {
            this.GuardArmy = new CreatureSet();
        }


        public string Message
        {
            get; set;
        }

        public CreatureSet GuardArmy
        {
            get; set;
        }

        public void AddStack(int slotIndex, StackDescriptor stack)
        {
            this.GuardArmy.Stacks.Add(stack);
        }

    }
}
