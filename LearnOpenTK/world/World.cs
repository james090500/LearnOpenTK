using OpenTK.Mathematics;

namespace LearnOpenTK.world
{
    public class World
    {
        private Chunk[] chunks = new Chunk[8]
        {
            new Chunk(new Vector2(0, 0)),
            new Chunk(new Vector2(0, 1)),
            new Chunk(new Vector2(1, 0)),
            new Chunk(new Vector2(1, 1)),
            null,
            null,
            null,
            null
        };

        public void Render()
        {        
            for(int i  = 0; i < chunks.Length; i++)
            {
                Chunk chunk = chunks[i];
                if(chunk != null)
                {
                    chunk.Render();
                }                
            }
        }
    }
}
