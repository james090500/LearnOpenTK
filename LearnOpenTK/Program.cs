namespace LearnOpenTK
{ 
    class Program
    {
        static void Main(String[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}