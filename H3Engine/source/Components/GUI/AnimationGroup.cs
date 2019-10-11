using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components.GUI
{
    public class AnimationGroup
    {
        public AnimationGroup()
        {
            this.Frames = new List<AnimationFrame>();

        }

        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public List<AnimationFrame> Frames
        {
            get;set;
        }
    }
}
