using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Core
{
    public class AbilitySkill
    {

        public AbilitySkill(ESecondarySkill skillId, ESecondarySkillLevel level)
        {

        }

        public ESecondarySkill Id
        {
            get; set;
        }

        public ESecondarySkillLevel Level
        {
            get; set;
        }

        public ESecondarySkillType Type
        {
            // Decided by the Skill Id
            get
            {
                return ESecondarySkillType.Adventure;
            }
        }
    }
}
