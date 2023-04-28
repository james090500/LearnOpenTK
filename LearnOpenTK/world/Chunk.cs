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

        private Dictionary<int, byte[,]> blocks = new Dictionary<int, byte[,]>();
        
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
            for (int y = 0; y < 80; y++)
            {
                byte[,] blockLayer = new byte[16,16];

                for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
                {
                    for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                    {                        
                        int realBlockX = (chunkX * CHUNK_SIZE) + x;
                        int realBlockZ = (chunkY * CHUNK_SIZE) + z;
                        int noisyHeight = waterHeight + (int)(noise.GetNoise(realBlockX, realBlockZ) * 15);

                        //Dont waste time on nothing
                        if (y > noisyHeight && y > waterHeight) continue;

                        Block block;

                        //Set the top block as long as we're about water
                        if (y == noisyHeight)
                        {
                            if (y > waterHeight)
                            {
                                block = new GrassBlock();
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
                            blockLayer[x, z] = Game.GetInstance().GetBlocks().GetIdFromBlock(block);
                        }
                    }
                }

                blocks.Add(y, blockLayer);
            }
        }

        public Block? GetBlock(int x, int y, int z)
        {
            byte[,]? chunkLayer = blocks.GetValueOrDefault(y);
            if (chunkLayer == null) return null;

            Block? block = Game.GetInstance().GetBlocks().GetBlockFromId(chunkLayer[x, z]);
            if(block != null)
            {
                block.Position = new Vector3((chunkX * CHUNK_SIZE) + x, y, (chunkY * CHUNK_SIZE) + z);
            }

            return block; 
        }

        public void SetBlock(int x, int y, int z, Block? block)
        {
            byte[,]? blockLayer = blocks.GetValueOrDefault(y);
            if (blockLayer == null)
            {
                blockLayer = new byte[16, 16];
            }

            blockLayer[x, z] = Game.GetInstance().GetBlocks().GetIdFromBlock(block);

            blocks[y] = blockLayer;
        }

        public Dictionary<int, byte[,]> GetBlocks() { return blocks; }

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
