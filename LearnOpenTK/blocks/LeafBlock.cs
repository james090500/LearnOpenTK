using OpenTK.Mathematics;

using static LearnOpenTK.renderers.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class LeafBlock : Block
    {
        public LeafBlock() {
            Transparent = true;
        }

        public override int GetTexturePosition(Face face)
        {
            return 7;
        }
    }
}
