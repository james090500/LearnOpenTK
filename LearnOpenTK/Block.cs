using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace LearnOpenTK
{
    internal class Block
    {
        private int vertexArrayHandle;
        private int indexBufferHandle;
        private int vertexBufferHandle;

        public float x;
        public float y;
        public float width;
        public float height;

        public VertexPositionColor[] vertices;
        public int[] indicies = new int[]
        {
            0, 1, 2, 0, 2, 3
        };


        public Block(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector2(x, y + height), new Color4(1f, 0f, 0f, 1f)),
                new VertexPositionColor(new Vector2(x + width, y + height), new Color4(0f, 1f, 0f, 1f)),
                new VertexPositionColor(new Vector2(x + width, y), new Color4(0f, 0f, 1f, 1f)),
                new VertexPositionColor(new Vector2(x, y), new Color4(1f, 1f, 0f, 1f))
            };
        }

        public void create()
        {
            //Sets the vertix handle to the buffer
            vertexBufferHandle = GL.GenBuffer();
            //Bind the buffer so we work on that buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //This sends the data to the buffer            
            GL.BufferData(BufferTarget.ArrayBuffer, this.length() * this.size(), this.vertices, BufferUsageHint.StaticDraw);
            //This unbinds the buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            indexBufferHandle = GL.GenBuffer();
            //Elements Array is an index buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, this.indicies.Length * sizeof(int), this.indicies, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);

            //This sort of binds to the buffer and explains how it works
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);

            foreach (var attr in VertexPositionColor.VertexInfo.VertexAttributes)
            {
                GL.VertexAttribPointer(attr.Index, attr.ComponentCount, VertexAttribPointerType.Float, false, this.size(), attr.Offset);
                GL.EnableVertexAttribArray(attr.Index);                
            }

            GL.BindVertexArray(0);
        }

        public void render()
        {
            GL.BindVertexArray(vertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);
            //We use 0 as we are already setting up elements avbove
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public int length()
        {
            return vertices.Length;
        }

        public int size()
        {
            return VertexPositionColor.VertexInfo.SizeInBytes;
        }

        public void destroy()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vertexArrayHandle);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(indexBufferHandle);

            //Free resources when program closed
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vertexBufferHandle);
        }
    }
}
