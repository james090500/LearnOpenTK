using OpenTK.Mathematics;
using System.Drawing;

namespace LearnOpenTK.world
{
    public class World
    {
        private int view = 6;

        private List<Chunk> activeChuks = new List<Chunk>();

        public void Render()
        {
            if(activeChuks.Count == 0)
            {
                for(int x = 0; x < 10; x++)
                {
                    for(int  y = 0; y < 10; y++)
                    {
                        activeChuks.Add(new Chunk(x, y));
                    }
                }
            }

            activeChuks.ForEach(ch =>
            {
                ch.Render();
            });
        }
        public void UpdateLoaded()
        {
            int playerRegionX = (int) Game.camera.Position.X / 16;
            int playerRegionZ = (int) Game.camera.Position.Z / 16;

            if(activeChuks.Count == 0)
            {
                Console.WriteLine("New World");
                activeChuks.Add(new Chunk(playerRegionX, playerRegionZ));
            }

            //Contains a rectangle of points around the players
            RectangleF rect = new RectangleF(
                playerRegionX - view,
                playerRegionZ - view,
                view * 2,
                view * 2
            );

            List<Chunk> newChunks = new List<Chunk>();
            activeChuks.ForEach((ch) =>
            {
                Point point = new Point(ch.chunkX, ch.chunkZ);
                if(rect.Contains(point))
                {
                    newChunks.Add(ch);
                } else
                {
                    ch.Unload();
                }
            });

            for (float x = rect.X; x < rect.Right; x++)
            {
                for (float y = rect.Y; y < rect.Bottom; y++)
                {
                    bool foundChunk = false;
                    activeChuks.ForEach((ch) => 
                    {
                        if(ch.chunkX == x && ch.chunkZ == y)
                        {
                            foundChunk = true;
                            return;
                        }
                    });

                    if(!foundChunk)
                    {
                        newChunks.Add(new Chunk((int) x, (int) y));
                    }
                }
            }
            activeChuks = newChunks;
        }
    }
}
