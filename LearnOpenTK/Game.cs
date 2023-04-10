using LearnOpenTK.textures;
using LearnOpenTK.world;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;

namespace LearnOpenTK
{
    public class Game : GameWindow
    {

        private static Game Instance;
        private Shader Shader;
        private Shader SpriteShader;
        private TextureManager TextureManager = new TextureManager();
        private readonly World World;
        private readonly Player Player;
        private readonly Controls Controls = new();

        Stopwatch stopwatch = new Stopwatch();
        int framesPerSecond = 0;

        public Game(string title, int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Title = title,
            Size = (width, height),
            StartVisible = false,
        })
        {
            stopwatch.Start();

            //Set instance
            Instance = this;

            // Load Shaders
            Shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
            SpriteShader = new Shader("shaders/2dshader.vert", "shaders/2dshader.frag");
           
            // Generate World
            World = new World();

            // Note that we're translating the scene in the reverse direction of where we want to move.
            Player = new Player();
            Player.Prepare();
        }

        /**
         * This runs on initialisation once
         */
        protected override void OnLoad()
        {
            base.OnLoad();

            CursorState = CursorState.Grabbed;

            GL.ClearColor(Color4.SkyBlue);
            GL.Enable(EnableCap.DepthTest | EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Load Textures
            TextureManager.loadTexture("stone", "assets/atlas.png");

            // Prepare the world
            World.GetChunksToRender();

            //Make window visible
            this.IsVisible = true;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Shader.Dispose();
            SpriteShader.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Load the shader
            Shader.Use();

            // Render the world
            World.Render();

            Shader.SetMatrix4("view", Player.GetCamera().GetViewMatrix());
            Shader.SetMatrix4("projection", Player.GetCamera().GetProjectionMatrix());

            // Render the player
            Player.Render();

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
            Controls.OnKeyboard(KeyboardState, args.Time);
            Controls.OnMouse(MouseState);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height); //Just used to resize the viewport which is what we render to
        }

        public static Game GetInstance()
        {
            return Instance;
        }

        public Shader GetShader()
        {
            return Shader;
        }

        public Shader GetSpriteShader()
        {
            return SpriteShader;
        }

        public TextureManager GetTextureManager()
        {
            return TextureManager;
        }

        public World GetWorld()
        {
            return World;
        }

        public Player GetPlayer()
        {
            return Player;
        }
    }
}
