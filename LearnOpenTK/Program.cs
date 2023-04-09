using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;

namespace LearnOpenTK
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (Game game = new Game("LearnOpenTK", 1280, 720))
            {
                game.Run();
            }
        }        
    }
}