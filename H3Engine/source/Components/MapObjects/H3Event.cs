using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.MapObjects
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
