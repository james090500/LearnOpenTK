﻿using LearnOpenTK.blocks;
using LearnOpenTK.renderers.model;
using LearnOpenTK.world;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static LearnOpenTK.renderers.model.BlockFace;

namespace LearnOpenTK.renderers.world
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
            PrepBlocks();
            PrepLiquid();
        }

        private void PrepBlocks()
        {
            //Generate mesh            
            blockMesh = new Mesh();
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < chunk.GetBlocks().Count; y++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {
                        Block? block = chunk.GetBlock(x, y, z);
                        if (block == null || block.Liquid) continue;

                        Model model = GetBlockModel();

                        //X Left and Right
                        if (ShouldRenderBlock(x - 1, y, z))
                        {
                            blockMesh.Update(model.GetFace(Face.BACK), x, y, z, block.GetTexturePosition(Face.BACK));
                        }
                        if (ShouldRenderBlock(x + 1, y, z))
                        {
                            blockMesh.Update(model.GetFace(Face.FRONT), x, y, z, block.GetTexturePosition(Face.FRONT));
                        }

                        //Y Up and Down
                        if (ShouldRenderBlock(x, y - 1, z))
                        {
                            blockMesh.Update(model.GetFace(Face.BOTTOM), x, y, z, block.GetTexturePosition(Face.BOTTOM));
                        }
                        if (ShouldRenderBlock(x, y + 1, z))
                        {
                            blockMesh.Update(model.GetFace(Face.TOP), x, y, z, block.GetTexturePosition(Face.TOP));
                        }

                        //Z Forward and Back
                        if (ShouldRenderBlock(x, y, z - 1))
                        {
                            blockMesh.Update(model.GetFace(Face.LEFT), x, y, z, block.GetTexturePosition(Face.LEFT));
                        }
                        if (ShouldRenderBlock(x, y, z + 1))
                        {
                            blockMesh.Update(model.GetFace(Face.RIGHT), x, y, z, block.GetTexturePosition(Face.RIGHT));
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
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < chunk.GetBlocks().Count; y++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {
                        Block? block = chunk.GetBlock(x, y, z);
                        if (block == null || !block.Liquid) continue;

                        Model model = GetLiquidModel();

                        //X Left and Right
                        if (ShouldRenderBlock(x - 1, y, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.BACK), x, y, z, block.GetTexturePosition(Face.BACK));
                        }
                        if (ShouldRenderBlock(x + 1, y, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.FRONT), x, y, z, block.GetTexturePosition(Face.FRONT));
                        }

                        //Y Up and Down
                        if (ShouldRenderBlock(x, y - 1, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.BOTTOM), x, y, z, block.GetTexturePosition(Face.BOTTOM));
                        }
                        if (ShouldRenderBlock(x, y + 1, z, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.TOP), x, y, z, block.GetTexturePosition(Face.TOP));
                        }

                        //Z Forward and Back
                        if (ShouldRenderBlock(x, y, z - 1, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.LEFT), x, y, z, block.GetTexturePosition(Face.LEFT));
                        }
                        if (ShouldRenderBlock(x, y, z + 1, true))
                        {
                            liquidMesh.Update(model.GetFace(Face.RIGHT), x, y, z, block.GetTexturePosition(Face.RIGHT));
                        }
                    }
                }
            }

            genVAO(liquidVAO, liquidMesh);
        }

        private bool ShouldRenderBlock(int x, int y, int z, bool isLiquid = false)
        {
            // Get the block
            Block? block;

            //If Y is out of bounds we know theres nothing in the chunk
            if (y <= 0 || y > chunk.GetBlocks().Count) return false;

                // A check to see if the item is out of bounds
            if (x < 0 || x >= Chunk.CHUNK_SIZE || z < 0 || z >= Chunk.CHUNK_SIZE)
            {
                int blockX = (chunk.chunkX * Chunk.CHUNK_SIZE) + x;
                int blockY = y;
                int blockZ = (chunk.chunkY * Chunk.CHUNK_SIZE) + z;

                if (x < 0)
                {
                    blockX = (chunk.chunkX * Chunk.CHUNK_SIZE) - 1;
                }
                else if (x >= Chunk.CHUNK_SIZE)
                {
                    blockX = (chunk.chunkX * Chunk.CHUNK_SIZE) + Chunk.CHUNK_SIZE;
                }

                if (z < 0)
                {
                    blockZ = (chunk.chunkY * Chunk.CHUNK_SIZE) - 1;
                }
                else if(z >= Chunk.CHUNK_SIZE)
                {
                    blockZ = (chunk.chunkY * Chunk.CHUNK_SIZE) + +Chunk.CHUNK_SIZE;
                }

                block = Game.GetInstance().GetWorld().GetBlockAt(blockX, blockY, blockZ);
            }
            else
            {
                block = chunk.GetBlock(x, y, z);
            }

            // If no block, render it
            if (block == null) { return true; }

            // If the block is transparent but not water
            if (!block.Transparent && !block.Liquid) { return false; }

            // Dont render liquid blocks insides
            if (block.Liquid && isLiquid) { return false; }

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
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            int texCoordLocation = Game.GetInstance().GetShader().GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation); //Enable the index
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            int lightLocation = Game.GetInstance().GetShader().GetAttribLocation("aLightValue");
            GL.EnableVertexAttribArray(lightLocation);
            GL.VertexAttribPointer(lightLocation, 1, VertexAttribPointerType.Float, false, 6 * sizeof(float), 5 * sizeof(float));
        }

        public bool IsVisible()
        {
            float centerX = (chunk.chunkX * Chunk.CHUNK_SIZE) + (Chunk.CHUNK_SIZE / 2);
            float centerY = (chunk.chunkY * Chunk.CHUNK_SIZE) + (Chunk.CHUNK_SIZE / 2);

            float sizeY = chunk.GetBlocks().Count;

            Vector3 chunkCenter = new Vector3(centerX, sizeY / 2, centerY);
            Vector3 chunkSize = new Vector3(Chunk.CHUNK_SIZE, sizeY, Chunk.CHUNK_SIZE);

            return Game.GetInstance().GetPlayer().GetCamera().Frustum.CubeInFrustum(chunkCenter, chunkSize);
        }

        public void Render()
        {
            if(IsVisible())
            {
                Game.GetInstance().GetShader().SetMatrix4("model", Matrix4.CreateTranslation(chunk.GetPosition()));

                GL.BindVertexArray(blockVAO);
                GL.DrawArrays(PrimitiveType.Triangles, 0, blockMesh.getTriangleCount());
            }

        }

        public void RenderLiquid()
        {
            if (IsVisible())
            {
                Game.GetInstance().GetShader().SetMatrix4("model", Matrix4.CreateTranslation(chunk.GetPosition()));

                GL.BindVertexArray(liquidVAO);
                GL.DrawArrays(PrimitiveType.Triangles, 0, liquidMesh.getTriangleCount());
            }
        }

        public void Unload()
        {
            GL.DeleteBuffer(blockVAO);
            GL.DeleteBuffer(liquidVAO);
        }
    }
}
