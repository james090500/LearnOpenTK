using LearnOpenTK.blocks;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnOpenTK.world
{
    internal class Chunk
    {
        private Vector3 Region;
        private Vector3 Size = new Vector3(16, 16, 16);
 
        public Chunk(Vector2 region) {
            this.Region = new Vector3(region.X, 0, region.Y);
            Console.WriteLine("First Block at " + (Region.X * Size.X) + 1 + ", " + (Region.Z * Size.Z) + 1);
        }

        public void Generate()
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int z = 0; z < Size.Z; z++)
                {
                    for (int y = 0; y < Size.Y; y++)
                    {
                        Vector3 position = new Vector3(x + (Region.X * Size.X), y, z + (Region.Z * Size.Z));
                        Matrix4 model = Matrix4.CreateTranslation(position);
                        Game.shader.SetMatrix4("model", model);
                        if (y >= 10)
                        {
                            new GrassBlock(position).render();
                        }
                        else
                        {
                            new StoneBlock(position).render();
                        }
                    }
                }
            }
        }
    }
}
