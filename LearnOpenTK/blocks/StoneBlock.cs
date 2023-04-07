using LearnOpenTK.model;
using OpenTK.Mathematics;

namespace LearnOpenTK.blocks
{
    internal class StoneBlock : Block
    {
        public StoneBlock(Vector3 position) : base(position)
        {
        }

        public override int GetTexturePosition(Face face)
        {
            return 0;
        }
    }
}
