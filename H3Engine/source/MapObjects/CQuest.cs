using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public CQuest Quest
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

    public class CGBoarderGuard : CGKeys, IQuestObject
    {

    }

    public class CGBorderGate : CGBoarderGuard
    {

    }



}
