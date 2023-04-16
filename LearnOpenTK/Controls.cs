using LearnOpenTK.blocks;
using LearnOpenTK.utils;
using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Linq.Expressions;

namespace LearnOpenTK
{
    public class Controls
    {
        Player player = Game.GetInstance().GetPlayer();

        Vector2 lastPos;
        bool firstMove;        
        const float sensitivity = 0.2f;
        const float jumpingSpeed = 5f;

        public void OnKeyboard(KeyboardState input)
        {
            double Time = Game.GetInstance().DeltaTime;

            if (input.IsKeyDown(Keys.Escape))
            {
                Game.GetInstance().Close();
            }

            Vector3 playerPos = player.GetPosition();
            Vector3 playerFront = new Vector3(player.GetCamera().Front.X, 0, player.GetCamera().Front.Z);

            if (!Game.GetInstance().GetWorld().GetChunkLoaded(playerPos)) return;

            /**
             * Sneaking and Sprinting
             */
            if (input.IsKeyDown(Keys.LeftShift))
            {
                player.SetSneaking(true);
            }                          
            if(input.IsKeyDown(Keys.LeftControl))
            {
                player.SetSprinting(true);
            }

            if (input.IsKeyReleased(Keys.LeftShift))
            {
                player.SetSneaking(false);
            }
            if (input.IsKeyReleased(Keys.LeftControl))
            {
                player.SetSprinting(false);
            }

            /**
             * Movement
             */
            if (input.IsKeyDown(Keys.W))
            {
                playerPos += playerFront * player.GetMovementSpeed() * (float)Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                playerPos -= (playerFront * player.GetMovementSpeed() * (float)Time);
            }
            if (input.IsKeyDown(Keys.A))
            {
                playerPos -= (player.GetCamera().Right * player.GetMovementSpeed() * (float)Time);
            }
            if (input.IsKeyDown(Keys.D))
            {
                playerPos += (player.GetCamera().Right * player.GetMovementSpeed() * (float)Time);
            }

            /**
             * Jumping and Falling
             */
            if (input.IsKeyPressed(Keys.Space))
            {
                player.SetJumping(true);
            }

            if (player.IsJumping())
            {
                playerPos.Y += (jumpingSpeed * (float)Time);
            }
            else
            {
                playerPos.Y -= (jumpingSpeed * (float)Time);
            }


            //Set player position
            player.GetCamera().Position = CollisionDetecion(playerPos) + player.GetHeight();

            //Debug
            if (input.IsKeyDown(Keys.F3))
            {
                DebugMenu.Toggle();
            }
        }

        public void OnMouse(MouseState mouse)
        {
            if (firstMove) // This bool variable is initially set to true.
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                player.GetCamera().Yaw += deltaX * sensitivity;
                player.GetCamera().Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }

            if (mouse.IsButtonPressed(MouseButton.Left))
            {
                player.OnLeftClick();
            }

            if (mouse.IsButtonPressed(MouseButton.Right))
            {
                player.OnRightClick();
            }
        }

        public void OnMouseWheel(float offset)
        {
            player.OnScrollWheel((int) offset);
        }

        private Vector3 CollisionDetecion(Vector3 newPos)
        {           
            Block? blockHead;
            Block? blockFeet;            
            Vector3 playerPos = player.GetPosition();
            Vector3 finalPos = playerPos;

            Vector3 newPosX = new(newPos.X, playerPos.Y, playerPos.Z);
            Vector3 newPosY = new(playerPos.X, newPos.Y, playerPos.Z);
            Vector3 newPosZ = new(playerPos.X, playerPos.Y, newPos.Z);

            blockHead = Game.GetInstance().GetWorld().GetBlockAt(newPosX + player.GetHeight());
            blockFeet = Game.GetInstance().GetWorld().GetBlockAt(newPosX);
            if ((blockHead == null || blockHead.Liquid) && (blockFeet == null || blockFeet.Liquid))
            {
                finalPos.X = newPos.X;
            }

            blockHead = Game.GetInstance().GetWorld().GetBlockAt(newPosY + player.GetHeight());
            blockFeet = Game.GetInstance().GetWorld().GetBlockAt(newPosY);
            if ((blockHead == null || blockHead.Liquid) && (blockFeet == null || blockFeet.Liquid))
            {
                finalPos.Y = newPos.Y;
            }

            blockHead = Game.GetInstance().GetWorld().GetBlockAt(newPosZ + player.GetHeight());
            blockFeet = Game.GetInstance().GetWorld().GetBlockAt(newPosZ);
            if ((blockHead == null || blockHead.Liquid) && (blockFeet == null || blockFeet.Liquid))
            {
                finalPos.Z = newPos.Z;
            }

            return finalPos;
        }
        
    }
}
