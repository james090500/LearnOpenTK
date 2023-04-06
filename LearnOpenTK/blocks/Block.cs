using LearnOpenTK.renderers;
using LearnOpenTK.textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LearnOpenTK.blocks
{
    public abstract class Block
    {
        private Vector3 position;
        private Texture texture;
        private bool visible = true;

        public Block(Vector3 position)
        {
            this.position = position;
        }
        
        public Vector3 getPosition()
        {
            return position;
        }
        
        public void setPosition(Vector3 position)
        {
            this.position = position;
        }

        public void setTexture(string textureName)
        {
            texture = Game.textureManager.getTexture(textureName);

            Game.shader.SetInt("texture", 0);
         
            texture.Use();
        }

        public void setVisible(bool visible)
        {
            this.visible = visible;
        }

        public void Render()
        {
            if (!visible) return;

            // Render the triangle/s
            //GL.BindVertexArray(BlockRenderer.getVAO());
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            texture.Use(TextureUnit.Texture0);
        }
    }
}
