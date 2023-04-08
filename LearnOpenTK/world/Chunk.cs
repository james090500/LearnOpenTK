using LearnOpenTK.blocks;
using LearnOpenTK.model;
using LearnOpenTK.renderers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static LearnOpenTK.model.BlockFace;

namespace LearnOpenTK.world
{
    internal class Chunk
    {
        private int vertexArrayObject;
        private Mesh mesh;
        public readonly int chunkX = 0;
        public readonly int chunkZ = 0;
        public readonly int chunkSize = 16;
        private int waterHeight = 64;
        private Block[,,] blocks = new Block[16, 100, 16];
        FastNoiseLite noise = new FastNoiseLite();

        public Chunk(int x, int z) {  
            this.chunkX = x;
            this.chunkZ = z;
            this.Generate();
        }

        private void Generate()
        {
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            noise.SetFrequency(0.02f);

            //Generate blocks
            Random random = new Random();
            for (int x = 0; x < chunkSize; x++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    int realBlockX = (chunkX * chunkSize) + x;
                    int realBlockZ = (chunkZ * chunkSize) + z;
                    int noisyHeight = 64 + (int)(noise.GetNoise(realBlockX, realBlockZ) * 10);
                    for (int y = 0; y < noisyHeight; y++)
                    {
                        Block block;
                        Vector3 blockPos = new Vector3(realBlockX, y, realBlockZ);

                        if (y == noisyHeight - 1 && noisyHeight > waterHeight)
                        {
                            block = new GrassBlock(blockPos);
                        }
                        else if(y == noisyHeight - 1 && noisyHeight == waterHeight)
                        {
                            block = new SandBlock(blockPos);
                        }
                        else if(y > noisyHeight - 5)
                        {
                            block = new DirtBlock(blockPos);
                        }
                        else
                        {
                            block = new StoneBlock(blockPos);
                        }
                        blocks[x, y, z] = block;
                    }

                    if(noisyHeight <= waterHeight)
                    {
                        for(int y = noisyHeight; y < waterHeight; y++)
                        {
                            Vector3 blockPos = new Vector3(realBlockX, y, realBlockZ);
                            blocks[x, y, z] = new WaterBlock(blockPos);
                        }
                    }
                }
            }

            //Generate mesh            
            mesh = new Mesh();
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        if (blocks[x, y, z] == null) continue;

                        Model model = BlockFace.GetBlockModel();
                        if (blocks[x,y,z].GetType() == typeof(WaterBlock))
                        {
                            model = BlockFace.GetLiquidModel();
                        }

                        //At the moment water is rendering its hidden sides
                        //We need to basically say
                        //ONLY RENDER IF NEXT TO NOTHING OR IF NEXT TO WATER AND IS NOT WATER

                        if (x - 1 < 0 || blocks[x - 1, y, z] == null || blocks[x - 1, y, z].IsTransparent())
                        {
                            if (blocks[x, y, z].GetType() != typeof(WaterBlock))
                            {
                                mesh.Update(model.GetFace(Face.BACK), x, y, z, blocks[x, y, z].GetTexturePosition(Face.BACK));
                            }
                        }
                        if (x + 1 > chunkSize - 1 || blocks[x + 1, y, z] == null || blocks[x + 1, y, z].IsTransparent())
                        {
                            if (blocks[x, y, z].GetType() != typeof(WaterBlock))
                            {
                                mesh.Update(model.GetFace(Face.FRONT), x, y, z, blocks[x, y, z].GetTexturePosition(Face.FRONT));
                            }
                        }
                        if (y - 1 < 0 || blocks[x, y - 1, z] == null || blocks[x, y - 1, z].IsTransparent())
                        {
                            if (blocks[x, y, z].GetType() != typeof(WaterBlock))
                            {
                                mesh.Update(model.GetFace(Face.BOTTOM), x, y, z, blocks[x, y, z].GetTexturePosition(Face.BOTTOM));
                            }
                        }
                        if (y + 1 > blocks.GetLength(1) - 1 || blocks[x, y + 1, z] == null || blocks[x, y + 1, z].IsTransparent())
                        {
                            if (blocks[x, y, z].GetType() != typeof(WaterBlock))
                            {
                                mesh.Update(model.GetFace(Face.TOP), x, y, z, blocks[x, y, z].GetTexturePosition(Face.TOP));
                            } else if(y + 1 > blocks.GetLength(1) - 1 || blocks[x, y + 1, z] == null)
                            {
                                mesh.Update(model.GetFace(Face.TOP), x, y, z, blocks[x, y, z].GetTexturePosition(Face.TOP));
                            }
                        }
                        if (z - 1 < 0 || blocks[x, y, z - 1] == null || blocks[x, y, z - 1].IsTransparent())
                        {
                            if (blocks[x, y, z].GetType() != typeof(WaterBlock))
                            {
                                mesh.Update(model.GetFace(Face.LEFT), x, y, z, blocks[x, y, z].GetTexturePosition(Face.LEFT));
                            }
                        }
                        if (z + 1 > chunkSize - 1 || blocks[x, y, z + 1] == null || blocks[x, y, z + 1].IsTransparent())
                        {
                            if (blocks[x, y, z].GetType() != typeof(WaterBlock))
                            {
                                mesh.Update(model.GetFace(Face.RIGHT), x, y, z, blocks[x, y, z].GetTexturePosition(Face.RIGHT));
                            }
                        }
                    }
                }
            }

            //Bind to the vertex array
            vertexArrayObject = GL.GenVertexArray();
            Console.WriteLine("HELLO " + vertexArrayObject);
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
            Game.shader.SetMatrix4("model", Matrix4.CreateTranslation(new Vector3(chunkX * chunkSize, 0, chunkZ * chunkSize)));
        }  
        
        public void Unload()
        {
            Console.WriteLine("RIP " + vertexArrayObject);
            GL.DeleteBuffer(vertexArrayObject);
        }
    }
}
