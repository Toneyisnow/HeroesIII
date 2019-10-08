using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.MapObjects
{
    public class ObjectTemplate
    {
        public enum EBlockMapBits
        {
            VISIBLE = 1,
            VISITABLE = 2,
            BLOCKED = 4
        }

        public EObjectType Type
        {
            get; set;
        }

        public int SubId
        {
            get; set;
        }

        public int PrintPriority
        {
            get; set;
        }

        public string AnimationFile
        {
            get; set;
        }

        public string EditorAnimationFile
        {
            get; set;
        }

        public string StringId
        {
            get; set;
        }

    }
}
