using H3Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3Engine.Components
{



    public class ArtifactSet
    {
        private Dictionary<EArtifactPosition, EArtifactId> artifacts;

        public bool CanPutAt(EArtifactId artifactId, EArtifactPosition position)
        {

            return true;
        }

        public void PutAt(EArtifactId artifactId, EArtifactPosition position)
        {

        }

        public void RemoveFrom(EArtifactId artifactId, EArtifactPosition position)
        {

        }


    }
}
