using OpenTK.Mathematics;
using static LearnOpenTK.renderers.model.BlockFace;

namespace LearnOpenTK.blocks
{
    public abstract class Block
    {
        public Vector3 Position { get; set; }
        public bool Transparent { get; set; } = false;
        public bool Breakable { get; set; } = true;
        public bool Liquid { get; set; } = false;

        public abstract int GetTexturePosition(Face face);
        public Block Clone()
        {
            return (Block)this.MemberwiseClone();
        }
    }
}
