using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.MapObjects
{
    public class CQuest
    {


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
