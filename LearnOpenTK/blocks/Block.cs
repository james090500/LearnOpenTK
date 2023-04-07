using LearnOpenTK.model;
using OpenTK.Mathematics;

namespace LearnOpenTK.blocks
{
    public abstract class Block
    {
        Vector3 position;

        public Block(Vector3 position)
        {
            this.position = position;
        }
        
        public void SetPosition(Vector3 position) { this.position = position; }
        public Vector3 GetPosition() { return position; }

        public abstract int GetTexturePosition(Face face);
    }
}
