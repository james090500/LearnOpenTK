using LearnOpenTK.model;
using OpenTK.Mathematics;
using static LearnOpenTK.model.BlockFace;

namespace LearnOpenTK.blocks
{
    internal class GrassBlock : Block
    {
        public GrassBlock(Vector3 position) : base(position)
        {
        }

        public override int GetTexturePosition(Face face)
        {
            return (face == Face.TOP) ? 2 : 3;
        }
    }
}
