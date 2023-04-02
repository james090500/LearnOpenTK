using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LearnOpenTK
{
    public class Game : GameWindow
    {
        private Block[] blocks = new Block[2];      
        private int shaderProgramHandle;

        public Game(string title = "Game1", int width = 1280, int height = 768)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Fixed,
                StartVisible = false,
                StartFocused = true,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            })
        {
            //Centre and resize screen
            this.CenterWindow();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            //When windows resized, make sure viewport is the entire window
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            this.IsVisible = true;

            //This doesn't clear, but tell it what we want it to be when we do clear
            //This of it as a "Setup" code until we tell it do "Apply"
            GL.ClearColor(Color4.SkyBlue);                   

            //This is shader code
            string vertexShaderCode =
                @"
                #version 330 core

                uniform vec2 ViewportSize;

                layout (location = 0) in vec2 aPosition;
                layout (location = 1) in vec4 aColor; 

                out vec4 vColor; 

                void main() {
                    float nx = aPosition.x / ViewportSize.x * 2f - 1f;
                    float ny = aPosition.y / ViewportSize.y * 2f - 1f;                    
                    gl_Position = vec4(nx, ny, 0, 1.0f);              

                    vColor = aColor;
                }
                ";

            //Define every pixel color. 
            string pixelShaderCode =
                @"
                #version 330 core
                
                in vec4 vColor;

                out vec4 pixelColor;

                void main() {
                    pixelColor = vColor;
                }
                ";

            //This creates a handle and compiles the code
            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);

            String vertexShaderLog = GL.GetShaderInfoLog(vertexShaderHandle);
            if(vertexShaderLog != String.Empty)
            {
                Console.WriteLine(vertexShaderLog);
            }

            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderHandle, pixelShaderCode);
            GL.CompileShader(pixelShaderHandle);

            String pixelShaderLog = GL.GetShaderInfoLog(pixelShaderHandle);
            if (pixelShaderLog != String.Empty)
            {
                Console.WriteLine(pixelShaderHandle);
            }

            //This is a shader program and we will attach shaders to the program
            shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(shaderProgramHandle, pixelShaderHandle);
            GL.LinkProgram(shaderProgramHandle);

            //We can dispose of this from memory once all set
            GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(shaderProgramHandle, pixelShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(pixelShaderHandle);

            //This below gets the viewport and passes the information to shader
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            GL.UseProgram(this.shaderProgramHandle);
            int viewportSizeUniformLocation = GL.GetUniformLocation(this.shaderProgramHandle, "ViewportSize");
            GL.Uniform2(viewportSizeUniformLocation, (float) viewport[2], (float) viewport[3]);
            GL.UseProgram(0);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            foreach (var block in blocks)
            {
                block.destroy();
            };

            GL.UseProgram(0);
            GL.DeleteProgram(shaderProgramHandle);

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        int block1 = 500;
        int block2 = 100;

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            block1 = (block1 > 1000) ? 0 : block1 + 1;
            block2 = (block2 > 1000) ? 0 : block2 + 1;

            blocks[0] = new Block(50, 120, block1, 250);
            blocks[1] = new Block(800, 80, block2, 100);
            foreach (var block in blocks)
            {
                block.create();
            }

            //We want to only clear the "Color" part (Back buffer)
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgramHandle);
            foreach (var block in blocks)
            {
                block.render();
            }

            //This brings the back buffer to the foreground so we can see
            this.Context.SwapBuffers();

            foreach (var block in blocks)
            {
                block.destroy();
            };
            base.OnRenderFrame(args);
        }
    }
}
