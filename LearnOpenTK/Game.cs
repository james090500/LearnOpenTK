using LearnOpenTK.blocks;
using LearnOpenTK.renderers;
using LearnOpenTK.textures;
using LearnOpenTK.world;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace LearnOpenTK
{
    public class Game : GameWindow {
        public static Shader shader;
        public static TextureManager textureManager = new TextureManager();
        public static World world;
        public static Camera camera;

        Vector2 lastPos;        
        bool firstMove;
        Stopwatch stopwatch = new();
        int framesPerSecond = 0;

        public Game(string title, int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Title = title,
            Size = (width, height),
            StartVisible = false,
        })
        {}

        /**
         * This runs on initialisation once
         */
        protected override void OnLoad()
        {
            base.OnLoad();

            CursorState = CursorState.Grabbed;            
            
            GL.ClearColor(Color4.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            // Load Textures
            textureManager.loadTexture("stone", "assets/atlas.png");
            //textureManager.loadTexture("grass", "assets/grass.png");

            // Load Shaders
            shader = new Shader("shaders/shader.vert", "shaders/shader.frag");

            // Load Blocks
            //BlockRenderer.Init();

            // Generate World
            world = new World();

            // Note that we're translating the scene in the reverse direction of where we want to move.
            camera = new Camera(new Vector3(50, 70, 50), Size.X / (float)Size.Y);

            stopwatch.Start();

            this.IsVisible = true;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            shader.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //world.UpdateLoaded();
            world.Render();      
            
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            SwapBuffers(); //Swaps from the live buffer to the next one

            framesPerSecond++;
            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
                stopwatch.Restart();
                Console.WriteLine(framesPerSecond + " FPS");
                framesPerSecond = 0;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            KeyboardState input = KeyboardState;
            if(input.IsKeyDown(Keys.Escape)) {
                Close();
            }

            const float cameraSpeed = 5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)args.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)args.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)args.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)args.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)args.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)args.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

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
                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height); //Just used to resize the viewport which is what we render to
        }
    }
}
