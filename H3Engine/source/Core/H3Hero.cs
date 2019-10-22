using H3Engine.Common;
using H3Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Core
{

    public class HeroSpecialty
    {
        public HeroSpecialty()
        {

        }

        public ESpecialtyType Type
        {
            get; set;
        }

        public int SubType
        {
            get; set;
        }

    }


    public class H3HeroId
    {
        public H3HeroId()
        {

        }

        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }
    

    public class HeroClass
    {
        public enum EClassAffinity
        {
            MIGHT,
            MAGIC
        };
        
        public EEthnicity Ethnicity
        {
            get; set;
        }

        public string Identifier
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }


    }


    public class H3Hero
    {

        public static H3Hero InitializeFromTemplate(int templateId)
        {

            return null;
        }


        public H3Hero()
        {

        }

        public string Identifier
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int PortaitIndex
        {
            get; set;
        }

        public string Biography
        {
            get; set;
        }

        public byte Sex
        {
            get; set;
        }
    

        public long Experience
        {
            get; set;
        }

        public int Level
        {
            get; set;
        }

        public int MaxExperience
        {
            get; set;
        }

        public int MaxLevel
        {
            get; set;
        }

        /// <summary>
        /// Will constantly be 4 members: Attack, Defence, Power, Intelligence
        /// </summary>
        public List<int> PrimarySkills
        {
            get; set;
        }

        /// <summary>
        /// Key: Skill Id Value: Level of Skill - (1 - basic, 2 - adv., 3 - expert)
        /// </summary>
        public List<AbilitySkill> SecondarySkills
        {
            get; set;
        }

        public List<ESpellId> Spells
        {
            get; set;
        }

        public ArtifactSet Artifacts
        {
            get; set;
        }
        
        public HeroSpecialty Specialty
        {
            get; set;
        }

        public CreatureSet Army
        {
            get; set;
        }




    }
}
