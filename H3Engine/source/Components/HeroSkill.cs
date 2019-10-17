using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components
{
    /// <summary>
    /// Handling the use of secondary skill
    /// </summary>
    public class HeroSkill
    {
        public HeroSkill(ESecondarySkill skillId, ESecondarySkillLevel level)
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
