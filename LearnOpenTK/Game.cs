using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace LearnOpenTK
{
    public class Game : GameWindow
    {
        float[] vertices = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f // top left
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        float[] texCoords =
        {
            0.0f, 0.0f,
            1.0f, 0.0f,
            0.0f, 1.0f,
            1.0f, 1.0f,
        };

        Shader shader;

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        public Game(string title, int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Title = title,
            Size = (width, height),
            StartVisible = false,
        })
        { }

        /**
         * This runs on initialisation once
         */
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(Color4.SkyBlue);

            //Load the shader
            shader = new Shader("shaders/shader.vert", "shaders/shader.frag");

            //Start of VBOs            
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); //We are binding to this buffer to following calls reference it
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); //Copy my data into the buffer

            //Bind to the vertex array
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);            

            //Bind the element buffer object. We can only bind if a VAO is bound
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //Explain to OpenGL how our vertices are layed out
            //Arguments below
            // 0 = Index (The location = 0 in the shader). We are telling what shader pos we want to fill
            // 3 = The size of the vertex attribute we are passing (Vec3 = 3)
            // Float because it is
            // Normalisation for larger data types
            // The "Stride" is when the next bit of data starts. As each point of x,y,z we know the next point is 3 floats away
            // The offset, but each value point as at the beginning. If we had a 4th point for color or something we could offset color to just read position
            GL.VertexAttribPointer(shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aPosition")); //Enable the index

            shader.Use();

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

            GL.Clear(ClearBufferMask.ColorBufferBit); //Clears the screen

            //Code for game
            shader.Use();

            // Render the triangle/s
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers(); //Swaps from the live buffer to the next one
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            KeyboardState input = KeyboardState;
            if(input.IsKeyDown(Keys.Escape)) {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height); //Just used to resize the viewport which is what we render to
        }
    }
}
