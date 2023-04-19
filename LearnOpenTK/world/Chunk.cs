﻿using LearnOpenTK.blocks;
using LearnOpenTK.renderers.world;
using OpenTK.Mathematics;

namespace LearnOpenTK.world
{
    public class Chunk
    {
        public readonly int chunkX = 0;
        public readonly int chunkY = 0;
        public static readonly int CHUNK_SIZE = 16;
        public int waterHeight = 64;
        public Block[,,] blocks = new Block[16, 100, 16];
        FastNoiseLite noise = new FastNoiseLite(Game.GetInstance().GetWorld().Seed);
        private ChunkRenderer chunkRenderer;

        public Chunk(int x, int z)
        {
            this.chunkX = x;
            this.chunkY = z;
            this.GenerateWorld();
            chunkRenderer = new ChunkRenderer(this);

        }

        private void GenerateWorld()
        {
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFrequency(0.03f);

            //Generate blocks
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    int realBlockX = (chunkX * CHUNK_SIZE) + x;
                    int realBlockZ = (chunkY * CHUNK_SIZE) + z;
                    int noisyHeight = 64 + (int)(noise.GetNoise(realBlockX, realBlockZ) * 15);
                    for (int y = 0; y < noisyHeight; y++)
                    {
                        Block block;
                        Vector3 blockPos = new Vector3(realBlockX, y, realBlockZ);

                        if (y == noisyHeight - 1 && noisyHeight > waterHeight)
                        {
                            block = new GrassBlock();
                        }
                        else if (y == noisyHeight - 1 && noisyHeight == waterHeight)
                        {
                            block = new SandBlock();
                        }
                        else if (y > noisyHeight - 5)
                        {
                            block = new DirtBlock();
                        }
                        else
                        {
                            block = new StoneBlock();                            
                        }
                        block.Position = blockPos;
                        blocks[x, y, z] = block;
                    }

                    if (noisyHeight <= waterHeight)
                    {
                        for (int y = noisyHeight; y < waterHeight; y++)
                        {
                            Vector3 blockPos = new Vector3(realBlockX, y, realBlockZ);
                            blocks[x, y, z] = new WaterBlock();
                            blocks[x, y, z].Position = blockPos;
                        }
                    }
                }
            }
        }

        public Vector3 GetPosition()
        {
            return new Vector3(chunkX * CHUNK_SIZE, 0, chunkY * CHUNK_SIZE);
        }

        public ChunkRenderer getChunkRenderer()
        {
            return this.chunkRenderer;
        }
    }
}
