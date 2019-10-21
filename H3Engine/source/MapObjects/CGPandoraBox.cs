using H3Engine.Common;
using H3Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class CGPandoraBox : ArmedInstance
    {
        public CGPandoraBox()
        {

        }

        public string BoxMessage
        {
            get; set;
        }

        /// <summary>
        /// helper - after battle even though we have no stacks, allows us to know that there was battle
        /// </summary>
        public bool hasGuardians
        {
            get;set;
        }

        //gained things:
        public uint GainExperience
        {
            get; set;
        }

        public uint ManaDiff
        {
            get; set;
        }

        public uint MoraleDiff
        {
            get; set;
        }

        public uint LuckDiff
        {
            get; set;
        }

        public ResourceSet GainResources
        {
            get; set;
        }

        public List<EArtifactId> GainArtifacts
        {
            get; set;
        }

        public List<int> GainPrimarySkills
        {
            get; set;
        }

        public List<ESecondarySkill> GainSecondarySkills
        {
            get; set;
        }

        public List<int> GainAbilityLevels
        {
            get; set;
        }

        public List<ESpellId> GainSpells
        {
            get; set;
        }

        public CreatureSet GainCreatures
        {
            get; set;
        }
        
    }
}
