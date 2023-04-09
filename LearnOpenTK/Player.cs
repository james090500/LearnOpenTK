using LearnOpenTK.blocks;
using LearnOpenTK.world;
using OpenTK.Mathematics;

namespace LearnOpenTK
{
    public class Player
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
            for (float i = 0; i < 4; i += 0.01f)
            {
                Vector3 value = GetCamera().Position + GetCamera().Front * i;

                int blockX = (int)value.X;
                int blockY = (int)value.Y;
                int blockZ = (int)value.Z;

                Block block = Game.GetInstance().GetWorld().GetBlockAt(blockX, blockY, blockZ);
                if (block != null && block.Breakable)
                {
                    Game.GetInstance().GetWorld().SetBlockAt(blockX, blockY, blockZ, null);
                    return;
                }
            }
        }

        public void OnRightClick()
        {
            for (float i = 0; i < 4; i += 0.01f)
            {
                Vector3 value = GetCamera().Position + GetCamera().Front * (i + 0.1f);
                int blockX = (int)value.X;
                int blockY = (int)value.Y;
                int blockZ = (int)value.Z;

                Block block = Game.GetInstance().GetWorld().GetBlockAt(blockX, blockY, blockZ);
                if (block != null)
                {
                    Vector3 newVal = GetCamera().Position + GetCamera().Front * (i + 0.01f);
                    Game.GetInstance().GetWorld().SetBlockAt((int) newVal.X, (int) newVal.Y, (int) newVal.Z, new StoneBlock(newVal));
                    return;
                }
            }
        }
    }
}
