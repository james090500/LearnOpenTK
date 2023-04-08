using LearnOpenTK.model;
using OpenTK.Mathematics;

using static LearnOpenTK.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class WaterBlock : Block
    {
        public WaterBlock(Vector3 position) : base(position)
        {
            SetTransparent(true);
        }

        public override int GetTexturePosition(Face face)
        {
            return 5;
        }
    }
}
