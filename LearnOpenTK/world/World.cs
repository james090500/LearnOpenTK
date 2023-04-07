using OpenTK.Mathematics;

namespace LearnOpenTK.world
{
    public class World
    {
        private Chunk[] chunks = new Chunk[9]
        {
            new Chunk(new Vector2(0, 0)),
            new Chunk(new Vector2(0, 1)),
            new Chunk(new Vector2(0, 2)),
            new Chunk(new Vector2(1, 0)),
            new Chunk(new Vector2(1, 1)),
            new Chunk(new Vector2(1, 2)),
            new Chunk(new Vector2(2, 0)),
            new Chunk(new Vector2(2, 1)),
            new Chunk(new Vector2(2, 2)),
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
