using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnOpenTK.blocks
{
    internal class StoneBlock : Block
    {
        public StoneBlock(Vector3 position) : base(position)
        {
            setTexture("walls");
        }
    }
}
