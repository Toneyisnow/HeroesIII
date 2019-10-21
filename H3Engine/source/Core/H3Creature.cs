using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Core
{
    public class H3Creature
    {
        public H3Creature()
        {

        }

        #region Basic Values

        public ECreatureId CreatureId
        {
            get; set;
        }

        public string Identifier
        {
            get; set;
        }

        public string NameReferenced
        {
            get; set;
        }

        public string NameSigular
        {
            get; set;
        }

        public string NamePlural
        {
            get; set;
        }

        public string AbilityDescription
        {
            get; set;
        }

        public int Faction
        {
            get; set;
        }

        public int Level
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ECreatureId> UpgradesTo
        {
            get; set;
        }

        #endregion

        #region Skill Values
        
        public int Attack
        {
            get; set;
        }

        public int Defense
        {
            get; set;
        }

        public int AmmunitionAmount
        {
            get; set;
        }

        public int Cost
        {
            get; set;
        }

        #endregion

        #region Battle Field

        public bool IsDoubleWide
        {
            get; set;
        }


        #endregion
    }
}
