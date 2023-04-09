using LearnOpenTK.blocks;
using LearnOpenTK.renderers.model;
using LearnOpenTK.renderers.world;
using LearnOpenTK.world;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static LearnOpenTK.renderers.model.BlockFace;

namespace LearnOpenTK.renderers
{
    public class ChunkRenderer
    {
        private readonly Chunk chunk;

        public int blockVAO = GL.GenVertexArray();
        private Mesh blockMesh;

        private int liquidVAO = GL.GenVertexArray();
        private Mesh liquidMesh;

        public ChunkRenderer(Chunk chunk)
        {
            this.chunk = chunk;
        }

        public void GenerateMesh()
        {
            this.PrepBlocks();
            this.PrepLiquid();
        }

        private void PrepBlocks()
        {
            //Generate mesh            
            blockMesh = new Mesh();
            for (int x = 0; x < chunk.blocks.GetLength(0); x++)
            {
                for (int y = 0; y < chunk.blocks.GetLength(1); y++)
                {
                    for (int z = 0; z < chunk.blocks.GetLength(2); z++)
                    {
                        if (chunk.blocks[x, y, z] == null || chunk.blocks[x, y, z].Liquid) continue;

                        Model model = BlockFace.GetBlockModel();

                        //X Left and Right
                        if (ShouldRenderBlock(x - 1, y, z))
                        {
                            blockMesh.Update(model.GetFace(Face.BACK), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.BACK));
                        }
                        if (ShouldRenderBlock(x + 1, y, z))
                        {
                            blockMesh.Update(model.GetFace(Face.FRONT), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.FRONT));
                        }

                        //Y Up and Down
                        if (ShouldRenderBlock(x, y - 1, z))
                        {
                            blockMesh.Update(model.GetFace(Face.BOTTOM), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.BOTTOM));
                        }
                        if (ShouldRenderBlock(x, y + 1, z))
                        {
                            blockMesh.Update(model.GetFace(Face.TOP), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.TOP));
                        }

                        //Z Forward and Back
                        if (ShouldRenderBlock(x, y, z - 1))
                        {
                            blockMesh.Update(model.GetFace(Face.LEFT), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.LEFT));
                        }
                        if (ShouldRenderBlock(x, y, z + 1))
                        {
                            blockMesh.Update(model.GetFace(Face.RIGHT), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.RIGHT));
                        }
                    }
                }
            }

            //Bind to the vertex array
            genVAO(blockVAO, blockMesh);
        }

        private void PrepLiquid()
        {
            //Generate mesh            
            liquidMesh = new Mesh();
            for (int x = 0; x < chunk.blocks.GetLength(0); x++)
            {
                for (int y = 0; y < chunk.blocks.GetLength(1); y++)
                {
                    for (int z = 0; z < chunk.blocks.GetLength(2); z++)
                    {
                        if (chunk.blocks[x, y, z] == null || !chunk.blocks[x, y, z].Liquid) continue;

                        Model model = BlockFace.GetLiquidModel();

                        //X Left and Right
                        if (ShouldRenderBlock(x - 1, y, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.BACK), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.BACK));
                        }
                        if (ShouldRenderBlock(x + 1, y, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.FRONT), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.FRONT));
                        }

                        //Y Up and Down
                        if (ShouldRenderBlock(x, y - 1, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.BOTTOM), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.BOTTOM));
                        }
                        if (ShouldRenderBlock(x, y + 1, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.TOP), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.TOP));
                        }

                        //Z Forward and Back
                        if (ShouldRenderBlock(x, y, z - 1, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.LEFT), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.LEFT));
                        }
                        if (ShouldRenderBlock(x, y, z + 1, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.RIGHT), x, y, z, chunk.blocks[x, y, z].GetTexturePosition(Face.RIGHT));
                        }
                    }
                }
            }

            genVAO(liquidVAO, liquidMesh);
        }

        private bool ShouldRenderBlock(int x, int y, int z, bool isLiquid = false)
        {         
            // Get the block
            Block block;

            // A check to see if the item is out of bounds
            if (
                (x < 0 || x > Chunk.CHUNK_SIZE - 1) ||
                (y < 0 || y > chunk.blocks.GetLength(1)) ||
                (z < 0 || z > Chunk.CHUNK_SIZE - 1)) {

                //int blockX = 0;
                //int blockZ = 0;

                //if (x < 0)
                //{
                //    blockX = (x + Chunk.CHUNK_SIZE) + ((chunk.chunkX - 1) * Chunk.CHUNK_SIZE);
                //}
                //else
                //{
                //    blockX = (x - Chunk.CHUNK_SIZE) + ((chunk.chunkX + 1) * Chunk.CHUNK_SIZE);
                //}

                //if (z < 0)
                //{
                //    blockZ = (z + Chunk.CHUNK_SIZE) + ((chunk.chunkY - 1) * Chunk.CHUNK_SIZE);
                //}
                //else
                //{
                //    blockZ = (z - Chunk.CHUNK_SIZE) + ((chunk.chunkY + 1) * Chunk.CHUNK_SIZE);
                //}

                //block = Game.GetInstance().GetWorld().GetBlockAt(blockX, y, blockZ);
                //The above sort of works, still weird artifacts tho and doesn't block when chunks are being genned
                return !isLiquid;
            } else {
                block = chunk.blocks[x, y, z];
            }            

            // If no block, render it
            if (block == null) { return true; }

            // If the block is transparent but not water
            if(!block.Transparent && !block.Liquid) { return false; }

            // Dont render liquid blocks insides
            if(block.Liquid && isLiquid) { return false; }

            return true;
        }

        private void genVAO(int vertexArrayObject, Mesh mesh)
        {
            //Bind to the vertexarray
            GL.BindVertexArray(vertexArrayObject);

            //Start of VBOs            
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject); //We are binding to this buffer to following calls reference it       
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.getVertices().Length * sizeof(float), mesh.getVertices(), BufferUsageHint.StaticDraw); //Copy my data into the buffer        

            //Bind the element buffer object. We can only bind if a VAO is bound
            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.getIndices().Length * sizeof(int), mesh.getIndices(), BufferUsageHint.StaticDraw);

            int vertexLocation = Game.GetInstance().GetShader().GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation); //Enable the index
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            int texCoordLocation = Game.GetInstance().GetShader().GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation); //Enable the index
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void Render()
        {
            Game.GetInstance().GetShader().SetMatrix4("model", Matrix4.CreateTranslation(new Vector3(chunk.chunkX * Chunk.CHUNK_SIZE, 0, chunk.chunkY * Chunk.CHUNK_SIZE)));

            GL.BindVertexArray(blockVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, blockMesh.getTriangleCount());
        }

        public void RenderLiquid()
        {
            Game.GetInstance().GetShader().SetMatrix4("model", Matrix4.CreateTranslation(new Vector3(chunk.chunkX * Chunk.CHUNK_SIZE, 0, chunk.chunkY * Chunk.CHUNK_SIZE)));

            GL.BindVertexArray(liquidVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, liquidMesh.getTriangleCount());
        }

        public void Unload()
        {
            GL.DeleteBuffer(blockVAO);
            GL.DeleteBuffer(liquidVAO);
        }
    }
}
