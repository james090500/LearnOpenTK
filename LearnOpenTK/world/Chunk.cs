using LearnOpenTK.blocks;
using LearnOpenTK.model;
using LearnOpenTK.renderers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LearnOpenTK.world
{
    internal class Chunk
    {
        private int vertexArrayObject;
        private Mesh mesh;
        private Vector2 Region;
        private readonly int size = 16;
        private readonly int height = 16;
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
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        Block block = null;
                        if (y < (height - 4))
                        {
                            block = new StoneBlock(new Vector3(x, y, z));
                        }
                        else if(y == height-1)
                        {
                            block = new GrassBlock(new Vector3(x, y, z));
                        }
                        else
                        {
                            block = new DirtBlock(new Vector3(x, y, z));
                        }
                        blocks[x, y, z] = block;
                    }
                }
            }

            //Generate mesh            
            mesh = new Mesh();
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        if (x - 1 < 0 || blocks[x - 1, y, z] == null)
                        {
                            mesh.Update(BlockModel.GetFace(Face.BACK), x, y, z, blocks[x,y,z].GetTexturePosition(Face.BACK));
                        }
                        if (x + 1 > size - 1 || blocks[x + 1, y, z] == null)
                        {
                            mesh.Update(BlockModel.GetFace(Face.FRONT), x, y, z, blocks[x, y, z].GetTexturePosition(Face.FRONT));
                        }
                        if (y - 1 < 0 || blocks[x, y - 1, z] == null)
                        {
                            mesh.Update(BlockModel.GetFace(Face.BOTTOM), x, y, z, blocks[x, y, z].GetTexturePosition(Face.BOTTOM));
                        }
                        if (y + 1 > height - 1 || blocks[x, y + 1, z] == null)
                        {
                            mesh.Update(BlockModel.GetFace(Face.TOP), x, y, z, blocks[x, y, z].GetTexturePosition(Face.TOP));
                        }
                        if (z - 1 < 0 || blocks[x, y, z - 1] == null)
                        {
                            mesh.Update(BlockModel.GetFace(Face.LEFT), x, y, z, blocks[x, y, z].GetTexturePosition(Face.LEFT));
                        }
                        if (z + 1 > size - 1 || blocks[x, y, z + 1] == null)
                        {
                            mesh.Update(BlockModel.GetFace(Face.RIGHT), x, y, z, blocks[x, y, z].GetTexturePosition(Face.RIGHT));
                        }
                    }
                }
            }

            Console.WriteLine("Vertices Done");

            //Bind to the vertex array
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            //Start of VBOs            
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject); //We are binding to this buffer to following calls reference it       
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.getVertices().Length * sizeof(float), mesh.getVertices(), BufferUsageHint.StaticDraw); //Copy my data into the buffer        

            //Bind the element buffer object. We can only bind if a VAO is bound
            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.getIndices().Length * sizeof(int), mesh.getIndices(), BufferUsageHint.StaticDraw);

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

            //TODO REMEMBER I AM SPLITTIGN TEXTURE AND VERTICES IN MESH
            int texCoordLocation = Game.shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation); //Enable the index
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        Random random = new Random();
        public void Render()
        {            
            GL.BindVertexArray(vertexArrayObject);                                  
            GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.getTriangleCount());
            Game.shader.SetMatrix4("model", Matrix4.CreateTranslation(new Vector3(Region.X * size, 0, Region.Y * size)));
        }        
    }
}
