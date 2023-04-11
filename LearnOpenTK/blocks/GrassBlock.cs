using OpenTK.Mathematics;
using static LearnOpenTK.renderers.model.BlockFace;

namespace LearnOpenTK.blocks
{
    internal class GrassBlock : Block
    {
        public override int GetTexturePosition(Face face)
        {
            return (face == Face.TOP) ? 2 : (face == Face.BOTTOM) ? 1 : 3;
        }
    }
}
