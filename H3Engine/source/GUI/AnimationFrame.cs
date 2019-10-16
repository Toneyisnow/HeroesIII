using H3Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.GUI
{
    public class AnimationFrame
    {
        public AnimationFrame()
        {

        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }

        public int FullWidth
        {
            get; set;
        }

        public int FullHeight
        {
            get; set;
        }

        public int LeftMargin
        {
            get; set;
        }

        public int TopMargin
        {
            get; set;
        }

        public byte[] Data
        {
            get; set;
        }

    }
}
