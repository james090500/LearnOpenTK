using OpenTK.Mathematics;

using static LearnOpenTK.renderers.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class LogBlock : Block
    {
        public override int GetTexturePosition(Face face)
        {
            return 6;
        }
    }
}
