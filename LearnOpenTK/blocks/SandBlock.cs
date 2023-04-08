using LearnOpenTK.model;
using OpenTK.Mathematics;

using static LearnOpenTK.model.BlockFace;
namespace LearnOpenTK.blocks
{
    internal class SandBlock : Block
    {
        public SandBlock(Vector3 position) : base(position)
        {
        }

        public override int GetTexturePosition(Face face)
        {
            return 4;
        }
    }
}
