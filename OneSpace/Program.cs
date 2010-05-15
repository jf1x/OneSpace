using System;

namespace OneSpace
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (OneSpace game = new OneSpace())
            {
                game.Run();
            }
        }
    }
}

