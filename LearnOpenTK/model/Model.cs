using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LearnOpenTK.model.BlockFace;

namespace LearnOpenTK.model
{
    public abstract class Model
    {
        public abstract float[] GetFace(Face face);
    }
}
