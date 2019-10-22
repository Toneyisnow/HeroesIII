using H3Engine.Components;
using System;
using System.Collections.Generic;

namespace H3Engine.MapObjects
{
    public class CQuest
    {
        public enum EMissionType
        {
            MISSION_NONE = 0,
            MISSION_LEVEL = 1,
            MISSION_PRIMARY_STAT = 2,
            MISSION_KILL_HERO = 3,
            MISSION_KILL_CREATURE = 4,
            MISSION_ART = 5,
            MISSION_ARMY = 6,
            MISSION_RESOURCES = 7,
            MISSION_HERO = 8,
            MISSION_PLAYER = 9,
            MISSION_KEYMASTER = 10
        };
        public enum EProgress
        {
            NOT_ACTIVE,
            IN_PROGRESS,
            COMPLETE
        };

        public int QuestId
        {
            get; set;
        }

        public EMissionType MissionType
        {
            get; set;
        }

        public EProgress Progress
        {
            get; set;
        }

        /// <summary>
        /// after this day (first day is 0) mission cannot be completed; if -1 - no limit
        /// </summary>
        public int LastDay
        {
            get; set;
        }

        public UInt32 M13489val
        {
            get; set;
        }

        public List<UInt32> M2Stats
        {
            get; set;
        }

        public List<UInt16> M5Artifacts
        {
            get; set;
        }

        public List<StackDescriptor> M6Creatures
        {
            get; set;
        }

        public List<UInt32> M7Resources
        {
            get; set;
        }
        
        // following fields are used only for kill creature/hero missions, the original
        // objects became inaccessible after their removal, so we need to store info
        // needed for messages / hover text
        public byte TextOption
        {
            get; set;
        }

        public byte CompletedOption
        {
            get; set;
        }


        //CStackBasicDescriptor stackToKill;
        //ui8 stackDirection;
        //std::string heroName; //backup of hero name
        //si32 heroPortrait;

        public string FirstVisitText
        {
            get; set;
        }

        public string NextVisitText
        {
            get; set;
        }

        public string CompletedText
        {
            get; set;
        }
        
        public bool IsCustomFirst
        {
            get; set;
        }

        public bool IsCustomNext
        {
            get; set;
        }

        public bool IsCustomComplete
        {
            get; set;
        }


    }

    public interface IQuestObject
    {
        CQuest Quest
        {
            get; set;
        }
    }

    public class CGSeerHut : ArmedInstance, IQuestObject
    {
        public enum ERewardType
        {
            NOTHING, EXPERIENCE, MANA_POINTS, MORALE_BONUS, LUCK_BONUS, RESOURCES, PRIMARY_SKILL, SECONDARY_SKILL, ARTIFACT, SPELL, CREATURE
        };

        public ERewardType RewardType
        {
            get; set;
        }


        public CQuest Quest
        {
            get; set;
        }

        public int RewardId
        {
            get; set;
        }

        public int RewardValue
        {
            get; set;
        }






    }

    public class CGQuestGuard : CGSeerHut
    {

    }

    public class CGKeys : CGObject
    {

    }

    public class CGKeyMasterTent : CGKeys
    {

    }

    public class CGBorderGuard : CGKeys, IQuestObject
    {
        public CQuest Quest
        {
            get; set;
        }
    }

    public class CGBorderGate : CGBorderGuard
    {

    }



}
