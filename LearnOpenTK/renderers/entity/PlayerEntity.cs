using OpenTK.Graphics.OpenGL;
using System.IO;

namespace LearnOpenTK.renderers.entity
{
    public class PlayerEntity
    {
        private int vertexBufferObject;
        private readonly float[] vertices = {
            -0.01f,  0.0f,  0.0f,
             0.01f,  0.0f,  0.0f,
             0.0f,   0.01f, 0.0f,
             0.0f,  -0.01f, 0.0f,
        };

        public void Prepare()
        {
            //Start of VBOs            
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject); //We are binding to this buffer to following calls reference it       
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); //Copy my data into the buffer        

            int vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            int vertexLocation = Game.GetInstance().GetSpriteShader().GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation); //Enable the index
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public void Render()
        {
            GL.Disable(EnableCap.DepthTest);            

            Game.GetInstance().GetSpriteShader().Use();
            GL.BindVertexArray(vertexBufferObject);           
            GL.DrawArrays(PrimitiveType.Lines, 0, 4);

            GL.Enable(EnableCap.DepthTest);
        }
    }
}
