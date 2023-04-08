using LearnOpenTK.model;
using OpenTK.Mathematics;

using static LearnOpenTK.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class DirtBlock : Block
    {
        public DirtBlock(Vector3 position) : base(position)
        {
        }

        public override int GetTexturePosition(Face face)
        {
            return 1;
        }
    }
}
