using LearnOpenTK.utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenTK
{
    public class Controls
    {
        Player player = Game.GetInstance().GetPlayer();

        Vector2 lastPos;
        bool firstMove;        
        const float sensitivity = 0.2f;
        const float movementSpeed = 5f;

        public void OnKeyboard(KeyboardState input)
        {
            double Time = Game.GetInstance().DeltaTime;

            if (input.IsKeyDown(Keys.Escape))
            {
                Game.GetInstance().Close();
            }

            Vector3 newPos = player.GetCamera().Position;
            if (input.IsKeyDown(Keys.W))
            {
                newPos += (player.GetCamera().Front * movementSpeed * (float)Time);
            }
            if (input.IsKeyDown(Keys.S))
            {
                newPos -= (player.GetCamera().Front * movementSpeed * (float)Time);
            }
            if (input.IsKeyDown(Keys.A))
            {
                newPos -= (player.GetCamera().Right * movementSpeed * (float)Time);
            }
            if (input.IsKeyDown(Keys.D))
            {
                newPos += (player.GetCamera().Right * movementSpeed * (float)Time);
            }
            if (input.IsKeyDown(Keys.Space))
            {
                newPos += (player.GetCamera().Up * movementSpeed * (float)Time);
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                newPos -= (player.GetCamera().Up * movementSpeed * (float)Time);
            }

            /**
             * Basic collision testing
             */
            bool notAtSolid = true;
            while (notAtSolid && Game.GetInstance().GetWorld().GetChunkLoaded(newPos))
            {
                Vector3 feetPos = newPos - new Vector3(0, 2.0f, 0);
                notAtSolid = (Game.GetInstance().GetWorld().GetBlockAt(feetPos) == null);
                if (notAtSolid)
                {
                    newPos -= new Vector3(0f, 0.01f, 0f);
                }
            }
            
            if (Game.GetInstance().GetWorld().GetBlockAt(newPos) == null && Game.GetInstance().GetWorld().GetChunkLoaded(newPos))
            {
                player.GetCamera().Position = newPos + new Vector3(0f, 0.1f, 0f);
            }
            //player.GetCamera().Position = newPos;

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
        
    }
}
