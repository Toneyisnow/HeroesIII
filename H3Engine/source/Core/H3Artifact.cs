using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Core
{
    public class H3Artifact
    {
        public H3Artifact(EArtifactId artId)
        {
            this.ArtifactId = artId;
        }

        public EArtifactId ArtifactId
        {
            get; set;
        }

        public bool IsBig()
        {
            return false;
        }
    }
}
