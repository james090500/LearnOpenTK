using LearnOpenTK.blocks;
using LearnOpenTK.renderers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LearnOpenTK.world
{
    internal class Chunk
    {
        private int vertexArrayObject;
        private Vector2 Region;
        private readonly int size = 16;
        private readonly int height = 16;
        private readonly int volume = 16 * 16 * 256;
        private Block[,,] blocks = new Block[16, 16, 16];
 
        public Chunk(Vector2 region) {
            this.Region = region;            
            this.Generate();
        }

        private void Generate()
        {
            //Generate blocks
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Block block = null;
                        if (y >= 0 && y <= 10)
                        {
                            block = new StoneBlock(new Vector3());
                        }
                        if (y >= 10 && y <= 16)
                        {
                            block = new GrassBlock(new Vector3());
                        }
                        blocks[x, y, z] = block;
                    }
                }
            }

            //Generate mesh            
            float[] tempMesh = { };
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (x > 0 && blocks[x - 1, y, z] != null)
                        {
                            tempMesh.Concat(BlockRenderer.left_vertices);
                        }
                        if (x > size && blocks[x + 1, y, z] != null)
                        {
                            tempMesh.Concat(BlockRenderer.right_vertices);
                        }
                        if (y > 0 && blocks[x, y - 1, z] != null)
                        {
                            tempMesh.Concat(BlockRenderer.bottom_vertices);
                        }
                        if (y > height && blocks[x, y + 1, z] != null)
                        {
                            tempMesh.Concat(BlockRenderer.top_vertices);
                        }
                        if (z > 0 && blocks[x, y, z - 1] != null)
                        {
                            tempMesh.Concat(BlockRenderer.back_vertices);
                        }
                        if (z > size && blocks[x, y, z + 1] != null)
                        {
                            tempMesh.Concat(BlockRenderer.front_vertices);
                        }
                    }
                }
            }

            tempMesh = tempMesh.ToArray();

            uint[] indices = {  // note that we start from 0!
                0, 1, 3,  // first triangle
                1, 2, 3    // second triangle
            };

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer); //We are binding to this buffer to following calls reference it
            GL.BufferData(BufferTarget.ArrayBuffer, tempMesh.Length * sizeof(float), tempMesh, BufferUsageHint.StaticDraw); //Copy my data into the buffer     

            int elementBufferObject = GL.GenBuffer();
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

        Random random = new Random();
        public void Render()
        {
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Matrix4 model = Matrix4.CreateTranslation(new Vector3(x + (Region.X * size), y, z + (Region.Y * size)));
                        Game.shader.SetMatrix4("model", model);

                        // Render the triangle/s
                        GL.BindVertexArray(vertexArrayObject);
                        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
                        blocks[x, y, z].Render();
                    }
                }
            }

        }
    }
}
