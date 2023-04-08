using LearnOpenTK.model;
using OpenTK.Mathematics;
using static LearnOpenTK.model.BlockFace;

namespace LearnOpenTK.blocks
{
    public abstract class Block
    {
        Vector3 position;
        bool Transparent;

        public Block(Vector3 position)
        {
            this.position = position;
        }
        
        public void SetPosition(Vector3 position) { this.position = position; }
        public Vector3 GetPosition() { return position; }

        public bool IsTransparent()
        {
            return Transparent;
        }

        public void SetTransparent(bool transparent)
        {
            this.Transparent = transparent;
        }

        public abstract int GetTexturePosition(Face face);
    }
}
