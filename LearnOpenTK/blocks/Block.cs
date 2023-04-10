using OpenTK.Mathematics;
using static LearnOpenTK.renderers.model.BlockFace;

namespace LearnOpenTK.blocks
{
    public abstract class Block
    {
        public Vector3 Position { get; }
        public bool Transparent { get; set; } = false;
        public bool Breakable { get; set; } = true;
        public bool Liquid { get; set; } = false;

        public Block(Vector3 position)
        {
            this.Position = position;
        }

        public abstract int GetTexturePosition(Face face);
    }
}
