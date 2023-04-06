using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace LearnOpenTK.renderers
{
    public class BlockRenderer : Renderer
    {
        private static int vertexArrayObject;
        private static int vertexBufferObject;
        private static int elementBufferObject;

        public readonly static float[] bottom_vertices =
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        };

        public readonly static float[] top_vertices =
        {
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
            0.0f, 1.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
        };

        public readonly static float[] left_vertices = {
            0.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 1.0f, 1.0f, 0.0f,
        };

        public readonly static float[] right_vertices = {
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
        };

        public readonly static float[] front_vertices = {
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        };

        public readonly static float[] back_vertices = {
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
            0.0f, 1.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f
        };

        private static uint[] indices = {  // note that we start from 0!
            0, 1, 3,  // first triangle
            1, 2, 3    // second triangle
        };

        public static void Init()
        {
            //Bind to the vertex array
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            //Start of VBOs            
            float[] vertices = bottom_vertices.Concat(top_vertices).Concat(front_vertices).Concat(back_vertices).Concat(left_vertices).Concat(right_vertices).ToArray();
            //vertexBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject); //We are binding to this buffer to following calls reference it
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); //Copy my data into the buffer        

            //Bind the element buffer object. We can only bind if a VAO is bound
            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //Load the shader
            Game.shader.Use();

            //Explain to OpenGL how our vertices are layed out
            //Arguments below
            // 0 = Index (The location = 0 in the shader). We are telling what shader pos we want to fill
            // 3 = The size of the vertex attribute we are passing (Vec3 = 3)
            // Float because it is
            // Normalisation for larger data types
            // The "Stride" is when the next bit of data starts. As each point of x,y,z we know the next point is 3 floats away
            // The offset, but each value point as at the beginning. If we had a 4th point for color or something we could offset color to just read position                        
            int vertexLocation = Game.shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation); //Enable the index
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            int texCoordLocation = Game.shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation); //Enable the index
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }
    }
}
