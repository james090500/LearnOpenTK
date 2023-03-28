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
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;

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

            //Define triangle vertices
            // X Y Z
            float[] vertices = new float[]
            {
                0.0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, //vertex 0 (Plus R, G, B A)
                0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, //vertex 1 (Plus R, G, B A)
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, //vertex 2 (Plus R, G, B A)
            };

            //Sets the vertix handle to the buffer
            vertexBufferHandle = GL.GenBuffer();
            //Bind the buffer so we work on that buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //This sends the data to the buffer            
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            //This unbinds the buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);

            //This sort of binds to the buffer and explains how it works
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            // 0 = location of shader, 3 = XYZ           
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);


            //This is shader code
            string vertexShaderCode =
                @"
                #version 330 core

                layout (location = 0) in vec3 aPosition;
                layout (location = 1) in vec4 aColor; 

                out vec4 vColor; 

                void main() {
                    vColor = aColor;
                    gl_Position = vec4(aPosition, 1.0f);                    
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

            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderHandle, pixelShaderCode);
            GL.CompileShader(pixelShaderHandle);

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

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vertexArrayHandle);

            //Free resources when program closed
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vertexBufferHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(shaderProgramHandle);

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //We want to only clear the "Color" part (Back buffer)
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgramHandle);
            GL.BindVertexArray(vertexArrayHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3); //Triangle has 3 sides

            //This brings the back buffer to the foreground so we can see
            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
