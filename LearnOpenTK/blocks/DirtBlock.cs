using OpenTK.Mathematics;

using static LearnOpenTK.renderers.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class DirtBlock : Block
    {
        public override int GetTexturePosition(Face face)
        {
            return 1;
        }
    }
}
