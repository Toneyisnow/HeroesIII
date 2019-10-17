using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class CGEvent : ArmedInstance
    {
        public CGEvent()
        {
            this.Abilities = new List<ESecondarySkill>();
        }

        public List<ESecondarySkill> Abilities
        {
            get; set;
        }


        public bool ComputerActivate
        {
            get; set;
        }

        public bool HumanActivate
        {
            get; set;
        }

        public bool RemoveAfterVisit
        {
            get; set;
        }

        public byte AvailableFor
        {
            get; set;
        }
    }
}
