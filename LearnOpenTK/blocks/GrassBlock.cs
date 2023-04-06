using OpenTK.Mathematics;

namespace LearnOpenTK.blocks
{
    internal class GrassBlock : Block
    {
        public GrassBlock(Vector3 position) : base(position)
        {
            setTexture("awesomeface");
        }
    }
}
