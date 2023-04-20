using LearnOpenTK.blocks;
using LearnOpenTK.renderers.entity;
using OpenTK.Mathematics;

namespace LearnOpenTK
{
    public class Player : PlayerEntity
    {

        private int scrollItem = 0;
        private Block holdingBlock = new StoneBlock();

        private Camera camera;

        private bool falling = false;
        private bool jumping = false;
        private bool sneaking = false;
        private bool sprinting = false;
        private Vector3 beforeJump;

        public bool NoClip = false;

        public Player()
        {
            camera = new Camera(new Vector3(0, 67, 0), Game.GetInstance().Size.X / (float)Game.GetInstance().Size.Y);
        }

        public Camera GetCamera() { return camera; }

        public Vector3 GetPosition() { return camera.Position - GetHeight(); }

        public void OnLeftClick()
        {
            Block? block = GetLookingAt(false);
            if(block != null && block.Breakable)
            {
                Game.GetInstance().GetWorld().SetBlockAt(block.Position, null);
            }
        }

        public void OnRightClick()
        {
            float looped = 0.0f;
            float distance = 5.0f;

            while (looped < distance)
            {
                //Floor to avoid automatic rounding
                float checkBlockX = (int)Math.Floor(GetCamera().Position.X + (GetCamera().Front.X * looped));
                float checkBlockY = (int)Math.Floor(GetCamera().Position.Y + (GetCamera().Front.Y * looped));
                float checkBlockZ = (int)Math.Floor(GetCamera().Position.Z + (GetCamera().Front.Z * looped));
                Vector3 checkBlockPos = new Vector3(checkBlockX, checkBlockY, checkBlockZ);

                Block? block = Game.GetInstance().GetWorld().GetBlockAt(checkBlockPos);
                if (block != null && !block.Liquid)
                {
                    float placeBlockX = (int)Math.Floor(GetCamera().Position.X + (GetCamera().Front.X * (looped - 0.5f)));
                    float placeBlockY = (int)Math.Floor(GetCamera().Position.Y + (GetCamera().Front.Y * (looped - 0.5f)));
                    float placeBlockZ = (int)Math.Floor(GetCamera().Position.Z + (GetCamera().Front.Z * (looped - 0.5f)));
                    Vector3 placeBlockPos = new Vector3(placeBlockX, placeBlockY, placeBlockZ);

                    Block placeBlock = holdingBlock.Clone();
                    placeBlock.Position = placeBlockPos;
                    Game.GetInstance().GetWorld().SetBlockAt(placeBlockPos, placeBlock);

                    break;
                }

                looped += 0.5f;
            }
        }

        public void OnScrollWheel(int offset)
        {
            scrollItem = (scrollItem - 1 + offset + 4) % 4 + 1;

            switch (scrollItem) {
                case 1:
                    holdingBlock = new StoneBlock();
                    break;
                case 2:
                    holdingBlock = new GrassBlock();
                    break;
                case 3:
                    holdingBlock = new DirtBlock();
                    break;
                case 4:
                    holdingBlock = new SandBlock();
                    break;
            }
        }

        public Block? GetLookingAt(bool includeLiquids = false)
        {
            float looped = 0.0f;
            float distance = 5.0f;
            
            while (looped < distance)
            {
                //Floor to avoid automatic rounding
                float blockX = (int) Math.Floor(GetCamera().Position.X + (GetCamera().Front.X * looped));
                float blockY = (int) Math.Floor(GetCamera().Position.Y + (GetCamera().Front.Y * looped));
                float blockZ = (int) Math.Floor(GetCamera().Position.Z + (GetCamera().Front.Z * looped));

                Vector3 blockPos = new Vector3(blockX, blockY, blockZ);

                Block? block = Game.GetInstance().GetWorld().GetBlockAt(blockPos);
                if (block != null && block.Liquid == includeLiquids)
                {
                    return block;
                }

                looped += 0.5f;
            }

            return null;
        }

        public bool IsFalling()
        {
            float blockAtY = (int) Math.Floor(GetPosition().Y);
            float playerPosY = GetPosition().Y;
            Block? block = Game.GetInstance().GetWorld().GetBlockAt(new(GetPosition().X, blockAtY - 1, GetPosition().Z));

            if ((playerPosY - blockAtY >= 0.02) || block == null || (block != null && block.Liquid))
            {
                falling = true;
            } 
            else
            {
                falling = false;
            }

            return falling && !jumping;
        }

        public void SetFalling(bool falling)
        {
            this.falling = falling;
        }

        public bool IsJumping()
        {
            if (this.jumping && GetPosition().Y < beforeJump.Y + 1.4f)
            {                
                return true;
            }
            else
            {
                this.jumping = false;
                return false;
            }
        }

        public void SetJumping(bool setJump)
        {
            this.beforeJump = GetPosition();
            jumping = setJump;
        }

        public void SetSneaking(bool sneaking)
        {
            this.sneaking = sneaking;
        }

        public bool IsSneaking()
        {
            return sneaking;
        }

        public void SetSprinting(bool sprinting)
        {
            this.sprinting = sprinting;
        }

        public bool IsSprinting()
        {
            return sprinting;
        }

        public bool IsSwimming()
        {
            Block? blockHead = Game.GetInstance().GetWorld().GetBlockAt(GetPosition() + GetHeight());
            Block? blockFeet = Game.GetInstance().GetWorld().GetBlockAt(GetPosition());
            return (blockHead != null && blockHead.Liquid) && (blockFeet != null && blockFeet.Liquid);
        }

        public float GetMovementSpeed()
        {
            float movementSpeed = 4f; 

            if(IsSprinting() && !IsSwimming())
            {
                movementSpeed += 2f;
            }

            if(sneaking)
            {
                movementSpeed -= 2f;
            }

            return movementSpeed;
        }

        public Vector3 GetHeight()
        {
            return new Vector3(0, 1.8f, 0);           
        }

        //public void GetCompass()
        //{
        //    // calculate the absolute value of x and z
        //    double absX = Math.Abs(GetCamera().Front.X);
        //    double absZ = Math.Abs(GetCamera().Front.Z);

        //    // calculate the direction to the closest cardinal point
        //    string direction;
        //    if (absX > absZ)
        //    {
        //        if (GetCamera().Front.X > 0) // east
        //        {
        //            direction = "East";
        //        }
        //        else // west
        //        {
        //            direction = "West";
        //        }
        //    }
        //    else
        //    {
        //        if (GetCamera().Front.Z > 0) // north
        //        {
        //            direction = "North";
        //        }
        //        else // south
        //        {
        //            direction = "South";
        //        }
        //    }

        //    Console.WriteLine("The closest cardinal direction is " + direction);
        //}
    }
}
