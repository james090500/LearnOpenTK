using LearnOpenTK.utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Linq.Expressions;

namespace LearnOpenTK
{
    public class Controls
    {

        Vector2 lastPos;
        bool firstMove;        
        const float sensitivity = 0.2f;
        const float movementSpeed = 5f;

        public void OnKeyboard(KeyboardState input, double Time)
        {
            Player player = Game.GetInstance().GetPlayer();

            if (input.IsKeyDown(Keys.Escape))
            {
                Game.GetInstance().Close();
            }

            if (input.IsKeyDown(Keys.W))
            {
                player.GetCamera().Position += player.GetCamera().Front * movementSpeed * (float)Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                player.GetCamera().Position -= player.GetCamera().Front * movementSpeed * (float)Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                player.GetCamera().Position -= player.GetCamera().Right * movementSpeed * (float)Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                player.GetCamera().Position += player.GetCamera().Right * movementSpeed * (float)Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                player.GetCamera().Position += player.GetCamera().Up * movementSpeed * (float)Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                player.GetCamera().Position -= player.GetCamera().Up * movementSpeed * (float)Time; // Down
            }
            if (input.IsKeyDown(Keys.F3))
            {
                DebugMenu.Toggle();
            }
        }

        public void OnMouse(MouseState mouse)
        {
            Player player = Game.GetInstance().GetPlayer();

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

    }
}
