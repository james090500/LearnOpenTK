using LearnOpenTK.blocks;
using OpenTK.Mathematics;

namespace LearnOpenTK.world
{
    public class World
    {
        private float viewDistance = 6.0f;
        private Vector2 knownCenter;

        private Dictionary<Vector2, Chunk> activeChuks = new Dictionary<Vector2, Chunk>();
        private Dictionary<Vector2, Chunk> newChunks = new Dictionary<Vector2, Chunk>();

        public void GetChunksToRender()
        {
            int regionX = (int)Math.Floor((double)Game.GetInstance().GetPlayer().GetPosition().X / Chunk.CHUNK_SIZE);
            int regionY = (int)Math.Floor((double)Game.GetInstance().GetPlayer().GetPosition().Z / Chunk.CHUNK_SIZE);
            Vector2 centerVec = new Vector2(regionX, regionY);

            if (activeChuks.Count > 0 && knownCenter.Equals(centerVec)) return;
            knownCenter = centerVec;

            List<Vector2> viewableVectors = new List<Vector2>();
            for (float x = centerVec.X - viewDistance; x <= centerVec.X + viewDistance; x++)
            {
                for (float y = centerVec.Y - viewDistance; y <= centerVec.Y + viewDistance; y++)
                {
                    Vector2 vector = new Vector2(x, y);
                    if (Vector2.Distance(centerVec, vector) <= viewDistance)
                    {
                        viewableVectors.Add(vector);
                        if (!activeChuks.ContainsKey(vector) && !newChunks.ContainsKey(vector))
                        {
                            Chunk chunk = new Chunk((int)x, (int)y);
                            newChunks.Add(vector, chunk);
                        }
                    }
                }
            }

            foreach (KeyValuePair<Vector2, Chunk> chunk in activeChuks)
            {
                //if vector not in viewableVectors - remove chunk
                Vector2 chunkVector = new Vector2(chunk.Value.chunkX, chunk.Value.chunkY);
                if (!viewableVectors.Contains(chunkVector))
                {
                    activeChuks[chunkVector].getChunkRenderer().Unload();
                    activeChuks.Remove(chunkVector);
                }
            }
        }

        public void Render()
        {
            GetChunksToRender();

            // Generate 1 Chunk mesh per frame
            if (newChunks.Count > 0)
            {
                KeyValuePair<Vector2, Chunk> chunk = newChunks.First();
                chunk.Value.getChunkRenderer().GenerateMesh();
                activeChuks.Add(chunk.Key, chunk.Value);
                newChunks.Remove(chunk.Key);
            }

            //Render all chunks
            foreach (KeyValuePair<Vector2, Chunk> chunk in activeChuks)
            {
                chunk.Value.getChunkRenderer().Render();
            }

            //Render the transparent water next
            foreach (KeyValuePair<Vector2, Chunk> chunk in activeChuks)
            {
                chunk.Value.getChunkRenderer().RenderLiquid();
            }
        }

        public Block? GetBlockAt(Vector3 blockPos)
        {
            return GetBlockAt(blockPos.X, blockPos.Y, blockPos.Z);
        }

        public Block? GetBlockAt(float x, float y, float z)
        {
            // Make sure Y is the min height
            if (y < 0) return null;

            int regionX = (int)Math.Floor(x / Chunk.CHUNK_SIZE);
            int regionY = (int)Math.Floor(z / Chunk.CHUNK_SIZE);

            Chunk chunk;
            if(!activeChuks.TryGetValue(new Vector2(regionX, regionY), out chunk))
            {
                return null;
            }

            int blockX = (int) Math.Floor(x - (regionX * Chunk.CHUNK_SIZE));
            int blockY = (int) Math.Floor(y);
            int blockZ = (int) Math.Floor(z - (regionY * Chunk.CHUNK_SIZE));

            if(blockX >= Chunk.CHUNK_SIZE || blockY >= chunk.blocks.GetLength(1) || blockZ >= Chunk.CHUNK_SIZE)
            {
                return null;
            } else
            {
                return chunk.blocks[blockX, blockY, blockZ];
            }
        }

        public void SetBlockAt(Vector3 position, Block block)
        {
            SetBlockAt((int) position.X, (int) position.Y, (int) position.Z, block);
        }        

        public void SetBlockAt(int x, int y, int z, Block block)
        {
            // Make sure Y is the min height
            if (y < 0) return;

            int regionX = (int)Math.Floor((double)x / Chunk.CHUNK_SIZE);
            int regionY = (int)Math.Floor((double)z / Chunk.CHUNK_SIZE);

            int blockX = x - (regionX * Chunk.CHUNK_SIZE);
            int blockY = y;
            int blockZ = z - (regionY * Chunk.CHUNK_SIZE);

            Chunk chunk;
            if (activeChuks.TryGetValue(new Vector2(regionX, regionY), out chunk))
            {
                if (block != null)
                {
                    block.Position = new Vector3(x, y, z);
                }

                chunk.blocks[blockX, blockY, blockZ] = block;
                chunk.getChunkRenderer().GenerateMesh();
            }
        }

        public bool GetChunkLoaded(Vector3 position)
        {
            // Make sure Y is the min height
            if (position.Y < 0) return false;

            int regionX = (int)Math.Floor((double) position.X / Chunk.CHUNK_SIZE);
            int regionY = (int)Math.Floor((double) position.Z / Chunk.CHUNK_SIZE);

            Chunk chunk;
            if (!activeChuks.TryGetValue(new Vector2(regionX, regionY), out chunk))
            {
                return false;
            } else
            {
                return chunk.blocks.Length > 0;
            }
        }
    }
}
