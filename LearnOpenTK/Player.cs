using LearnOpenTK.blocks;
using LearnOpenTK.renderers.entity;
using OpenTK.Mathematics;

namespace LearnOpenTK
{
    public class Player : PlayerEntity
    {
        private Camera camera;

        public Player()
        {
            camera = new Camera(new Vector3(0, 70, 0), Game.GetInstance().Size.X / (float)Game.GetInstance().Size.Y);
        }

        public Camera GetCamera() { return camera; }

        public Vector3 GetPosition() { return camera.Position; }

        public void OnLeftClick()
        {
            Block block = GetLookingAt();
            if(block != null && block.Breakable)
            {
                Game.GetInstance().GetWorld().SetBlockAt(block.Position, null);
            }
        }

        public void OnRightClick()
        {
            float looped = 0.0f;
            float distance = 5.0f;
            float unitSize = 1.0f;

            float directionX = GetCamera().Front.X;
            float directionY = GetCamera().Front.Y;
            float directionZ = GetCamera().Front.Z;

            while (looped < distance)
            {
                float blockX = GetPosition().X + (directionX * (unitSize * (looped + unitSize)));
                float blockY = GetPosition().Y + (directionY * (unitSize * (looped + unitSize)));
                float blockZ = GetPosition().Z + (directionZ * (unitSize * (looped + unitSize)));

                Vector3 blockPos = new Vector3(blockX, blockY, blockZ);
                Block? block = Game.GetInstance().GetWorld().GetBlockAt(blockPos);

                if (block != null && !block.Liquid)
                {
                    float placeableX = GetPosition().X + (directionX * (unitSize * looped));
                    float placeableY = GetPosition().Y + (directionY * (unitSize * looped));
                    float placeableZ = GetPosition().Z + (directionZ * (unitSize * looped));
                    
                    Vector3 placeablePos = new Vector3(placeableX, placeableY, placeableZ);
                    Game.GetInstance().GetWorld().SetBlockAt(placeablePos, new StoneBlock(placeablePos));
                    return;
                }
                looped++;
            }
        }

        public Block GetLookingAt()
        {
            float looped = 0.0f;
            float distance = 5.0f;
            float unitSize = 1.0f;

            float directionX = GetCamera().Front.X;
            float directionY = GetCamera().Front.Y;
            float directionZ = GetCamera().Front.Z;

            while (looped < distance)
            {
                //If facing X
                float blockX = GetPosition().X;
                float blockY = GetPosition().Y + Math.Sign(directionY * looped);
                float blockZ = GetPosition().Z;

                if (directionX > directionZ)
                {
                    blockX = GetPosition().X + Math.Sign(directionX * looped);
                } else
                {
                    blockZ = GetPosition().Z + Math.Sign(directionZ * looped);
                }
                

                //If I am looking at Z Plane, the X Value is changing when it shouldnt

                Console.WriteLine("Player at (" + GetPosition().X + ", " + GetPosition().Y + ", " + GetPosition().Z + ") --- Checking Block at (" + blockX + ", " + blockY + ", " + blockZ + ")");

                Vector3 blockPos = new Vector3(blockX, blockY, blockZ);

                Block? block = Game.GetInstance().GetWorld().GetBlockAt(blockPos);
                if (block != null)
                {
                    Console.WriteLine(block);
                    return block;
                }

                looped += 0.01f;
            }

            return null;
        }

        public void GetCompass()
        {
            // calculate the absolute value of x and z
            double absX = Math.Abs(GetCamera().Front.X);
            double absZ = Math.Abs(GetCamera().Front.Z);

            // calculate the direction to the closest cardinal point
            string direction;
            if (absX > absZ)
            {
                if (GetCamera().Front.X > 0) // east
                {
                    direction = "East";
                }
                else // west
                {
                    direction = "West";
                }
            }
            else
            {
                if (GetCamera().Front.Z > 0) // north
                {
                    direction = "North";
                }
                else // south
                {
                    direction = "South";
                }
            }

            Console.WriteLine("The closest cardinal direction is " + direction);
        }
    }
}
