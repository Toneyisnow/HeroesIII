using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Core
{
    public class PlayerInfo
    {
        public PlayerInfo()
        {

        }

        #region Properties

        public int DefaultCastle
        {
            get
            {
                return 0;
            }
        }

        public int DefaultHero
        {
            get
            {
                return 0;
            }
        }

        public bool CanHumanPlay
        {
            get; set;
        }

        public bool CanComputerPlay
        {
            get; set;
        }

        public EAiTactic AiTactic
        {
            get; set;
        }

        public List<int> AllowedFactions
        {
            get; set;
        }

        public bool IsFactionRandom
        {
            get; set;
        }

        /// <summary>
        /// VCMI games only
        /// </summary>
        public string MainHeroInstance
        {
            get; set;
        }

        public bool HasRandomHero
        {
            get; set;
        }

        public int MainCustomHeroPortrait
        {
            get; set;
        }

        public string MainCustomHeroName
        {
            get; set;
        }

        public int MainCustomHeroId
        {
            get; set;
        }

        /// <summary>
        /// /// list of placed heroes on the map
        /// </summary>
        public List<H3HeroId> HeroIds
        {
            get; set;
        }

        public bool HasMainTown
        {
            get; set;
        }

        public bool GenerateHeroAtMainTown
        {
            get; set;
        }

        public MapPosition MainTownPosition
        {
            get; set;
        }

        public int TeamId
        {
            get; set;
        }

        public bool GenerateHero
        {
            get; set;
        }

        /// <summary>
        ///  Unused
        /// </summary>
        public int P7
        {
            get; set;
        }

        public int PowerPlaceHolders
        {
            get; set;
        }


        #endregion

        #region Public Methods

        public bool CanAnyonePlay()
        {
            return false;
        }

        public bool HasCustomMainHero()
        {
            return false;
        }

        #endregion

    }
}
