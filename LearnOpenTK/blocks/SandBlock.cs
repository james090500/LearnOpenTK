using OpenTK.Mathematics;

using static LearnOpenTK.renderers.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class SandBlock : Block
    {
        public override int GetTexturePosition(Face face)
        {
            return 4;
        }
    }
}
