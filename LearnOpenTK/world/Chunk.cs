using LearnOpenTK.blocks;
using LearnOpenTK.renderers.world;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace LearnOpenTK.world
{
    public class Chunk
    {
        public readonly int chunkX = 0;
        public readonly int chunkY = 0;
        public static readonly int CHUNK_SIZE = 16;
        public int waterHeight = 64;

        private Dictionary<int, Block[,]> blocks = new Dictionary<int, Block[,]>();
        
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
            int noisyHeight = 1;

            for (int y = 0; y < 80; y++)
            {
                Block[,] blockLayer = new Block[16,16];

                for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {                        
                        int realBlockX = (chunkX * CHUNK_SIZE) + x;
                        int realBlockZ = (chunkY * CHUNK_SIZE) + z;
                        noisyHeight = waterHeight + (int)(noise.GetNoise(realBlockX, realBlockZ) * 15);

                        //Dont waste time on nothing
                        if (y > noisyHeight && y > waterHeight) continue;

                        Block? block = null;
                        Vector3 blockPos = new Vector3(realBlockX, y, realBlockZ);

                        //Set the top block as long as we're about water
                        if (y == noisyHeight)
                        {
                            if (y > waterHeight)
                            {
                                block = new GrassBlock();
                                Random random = new Random();
                                if (random.Next(250) == 1)
                                {
                                    for (int i = 0; i < 6; i++) {
                                        SetBlock(x, y + i, z, new LogBlock());
                                        if(i >= 3)
                                        {
                                            for(int j = -2; j < 3; j++)
                                            {
                                                for (int k = -2; k < 3; k++)
                                                {
                                                    if (x + j > 0 && x + j <= 15 && z + k > 0 && z + k <= 15)
                                                    {
                                                        SetBlock(x + j, y + i, z + k, new LeafBlock());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                block = new SandBlock();
                            }
                        }
                        else if (y <= waterHeight && y > noisyHeight)
                        {
                            block = new WaterBlock();
                        }
                        else if (y > noisyHeight - 4)
                        {
                            block = new DirtBlock();
                        }
                        else
                        { 
                            block = new StoneBlock();
                        }

                        if (block != null)
                        {
                            block.Position = blockPos;
                            blockLayer[x, z] = block;
                        }
                    }
                }

                Block[,] testBlockLayer = blocks.GetValueOrDefault(y);
                if(testBlockLayer != null)
                {
                    for(int x = 0; x < testBlockLayer.GetLength(0); x++)
                    {
                        for (int z = 0; z < testBlockLayer.GetLength(1); z++)
                        {
                            if (testBlockLayer[x, z] != null)
                            {
                                blockLayer[x, z] = testBlockLayer[x, z];
                            }
                        }
                    }
                    blocks.Remove(y);
                }
                blocks.Add(y, blockLayer);
            }
        }

        public Block? GetBlock(int x, int y, int z)
        {
            Block[,]? chunkLayer = blocks.GetValueOrDefault(y);
            if (chunkLayer == null) return null;

            return chunkLayer[x, z];
        }

        public void SetBlock(int x, int y, int z, Block? block)
        {
            Block[,]? chunkLayer = blocks.GetValueOrDefault(y);
            if (chunkLayer == null)
            {
                chunkLayer = new Block[16, 16];
                blocks.Add(y, chunkLayer);
            }

            chunkLayer[x, z] = block;
        }

        public Dictionary<int, Block[,]> GetBlocks() { return blocks; }

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
