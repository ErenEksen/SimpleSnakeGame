
using SnakeGame;

namespace Program
{
    public static class Starter
    {
        public static int Main(string[] args)
        {
            var snake = new Snake();
            
            snake.StartGame();
            return 0;
        }
    }
}