using System.Runtime.InteropServices;

namespace SnakeGame;

internal class Snake
{
    private readonly char _borderChar = '#';
    private readonly char _foodChar = '*';
    private readonly char[] _characterHead = { 'X', 'X', 'X', 'X' };
    private readonly char _characterBody = '#';
    int _xBorderSize = 100;
    int _yBorderSize = 30;
    
    private enum SnakeDirection {Right, Left, Up, Down}
    private SnakeDirection snakeDir = SnakeDirection.Right;
    private int xCharacterPosition, yCharacterPosition;
    private int xFoodPosition, yFoodPosition;
    private int[] xBodyPosition = new int[1], yBodyPosition = new int[1];
    private int snakeLenght = 1;
    private bool canMove;
    public void StartGame(CancellationToken token = default)
    {
        Console.Clear();
        sizeOfTheGameUserInput();
        PrintBorder();
        DrawChar();
        FoodSpawn();
        keyReader();
        CharacterMovements();
        EndOfTheLine();
        
        Thread.Sleep(Timeout.Infinite);
    }

    void sizeOfTheGameUserInput()
    {
        string WidthText = "Please write the width of the playground (Default 75) : "
            , HeightText = "Please write the height of the playground (Default 30) : ";
        
        Console.Write(WidthText);
        string input = Console.ReadLine();
        _xBorderSize = string.IsNullOrEmpty(input) ? 75 : Convert.ToInt32(input);
        Console.SetCursorPosition(0,0);
        for (int i = 0; i < 75; i++)  Console.Write(' ');
        Console.SetCursorPosition(0,0);
        
        Console.Write(HeightText);
        input = Console.ReadLine();
        _yBorderSize = string.IsNullOrEmpty(input) ? 30 : Convert.ToInt32(input);
        Console.SetCursorPosition(0,0);
        for (int i = 0; i < 75; i++)  Console.Write(' ');
        Console.SetCursorPosition(0,0);
    }
    

    //Checks the key that I pressed on keyboard.
    //Also prevents that snake to turn 180 degree
    private void keyReader()
    {
        Task.Factory.StartNew(() =>
        {
            start:
            ConsoleKey input = Console.ReadKey().Key;
            if (!canMove) goto start;
            canMove = false;
            switch (input)
            {
                case ConsoleKey.UpArrow:
                    if (snakeDir == SnakeDirection.Down) break;
                    snakeDir = SnakeDirection.Up;
                    break;
            
                case ConsoleKey.DownArrow:
                    if (snakeDir == SnakeDirection.Up) break;
                    snakeDir = SnakeDirection.Down;
                    break;
            
                case ConsoleKey.RightArrow:
                    if (snakeDir == SnakeDirection.Left) break;
                    snakeDir = SnakeDirection.Right;
                    break;
                case ConsoleKey.LeftArrow:
                    if (snakeDir == SnakeDirection.Right) break;
                    snakeDir = SnakeDirection.Left;
                    break;
            }

            goto start;
        });


        //keyReader();
    }
    
    // To write a char to spesific coordinate with single line of code.
    private void WriteAt(char Character, int x, int y)
    {
        Console.SetCursorPosition(x,y);
        Console.Write(Character);
    }
    // To write a string after spesific coordinate with single line of code.
    private void WriteAtString(string text, int x, int y)
    {
        Console.SetCursorPosition(x,y);
        Console.Write(text);
    }
    
    //To print the border of the game
    private void PrintBorder()
    {
        Console.WriteLine();
        for (int i = 0; i < _xBorderSize; i++)
        {
            Console.Write(_borderChar);
        }

        for (int i = 0; i < _yBorderSize; i++)
        {
            WriteAt(_borderChar,0,i);
            WriteAt(_borderChar,_xBorderSize - 1,i);
        }
        Console.Write('\n');
        for (int i = 0;i < _xBorderSize; i++)
        {
            Console.Write(_borderChar);
        }

        _yBorderSize = Console.CursorTop;
        _xBorderSize = Console.CursorLeft;
    }

    //it draws the char for the first time game starts.
    private void DrawChar()
    {
        xCharacterPosition = (_xBorderSize / 2);
        xBodyPosition[0] = xCharacterPosition - 1;
        yBodyPosition[0] = yCharacterPosition = (_yBorderSize / 2);
        
        WriteAt(_characterHead[0], xCharacterPosition, yCharacterPosition);
        WriteAt(_characterBody, xBodyPosition[0], yBodyPosition[0]);
        
    }

    //After eating apple, it adds one body part to character
    private void MakeCharacterBigger(int newBodyPartX, int newBodyPartY)
    {

        snakeLenght++;
        Array.Resize<int>(ref xBodyPosition, snakeLenght);
        Array.Resize<int>(ref yBodyPosition, snakeLenght);
        WriteAt(_characterBody, newBodyPartX, newBodyPartY);
        xBodyPosition[snakeLenght - 1] = newBodyPartX;
        yBodyPosition[snakeLenght - 1] = newBodyPartY;


    }
    

    private void FoodSpawn()
    {
        foodSpawnAgain:
        var r = new Random();

        //in Windows, Y coordinate of the Windows terminal starts from index 1.
        //But in Linux/MacOS/Unix terminal, it just starts at index 0.
        //So Food Spawner just changing behavior for windows terminal.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) yFoodPosition = r.Next(2, _yBorderSize);
        else yFoodPosition = r.Next(1, _yBorderSize);
        xFoodPosition = r.Next(1, _xBorderSize - 1);
        

        // To preventing that food to spawn inside of character.
        if(xFoodPosition == xCharacterPosition) goto foodSpawnAgain;
        for (int i = 0; i < snakeLenght; i++)
            if (xBodyPosition[i] == xCharacterPosition & yBodyPosition[i] == yCharacterPosition)
                goto foodSpawnAgain;
        
        
        
        WriteAt(_foodChar, xFoodPosition, yFoodPosition);
        
        EndOfTheLine();
    }

    
    private async Task CharacterMovements()
    {
        while (true)
        {
            await Task.Delay(200);
            canMove = true;
            int tempBodyX = xBodyPosition[snakeLenght - 1];
            int tempBodyY = yBodyPosition[snakeLenght - 1];
            switch (snakeDir)
            {
                case SnakeDirection.Right:
                    WriteAt(' ', tempBodyX, tempBodyY); //The last part of the tail will remove.

                    //The last part of the tail will swap the place with previous head position
                    xBodyPosition[snakeLenght - 1] = xCharacterPosition;
                    yBodyPosition[snakeLenght - 1] = yCharacterPosition;
                   
                    //Array elements shifts by one.
                    for (int i = snakeLenght - 1; i > 0; i--)
                    {
                        (xBodyPosition[i], xBodyPosition[i - 1]) = (xBodyPosition[i - 1], xBodyPosition[i]);
                        (yBodyPosition[i], yBodyPosition[i - 1]) = (yBodyPosition[i - 1], yBodyPosition[i]);
                    }
                    
                    xCharacterPosition++;

                    //Draws the new position.
                    WriteAt(_characterBody, xBodyPosition[0]  , yBodyPosition[0] );
                    WriteAt(_characterHead[0], xCharacterPosition, yCharacterPosition);
                    break;
                case SnakeDirection.Left:
                    WriteAt(' ', tempBodyX, tempBodyY);
                    
                    xBodyPosition[snakeLenght - 1] = xCharacterPosition;
                    yBodyPosition[snakeLenght - 1] = yCharacterPosition;
                    
                    for (int i = snakeLenght - 1; i > 0; i--)
                    {
                        (xBodyPosition[i], xBodyPosition[i - 1]) = (xBodyPosition[i - 1], xBodyPosition[i]);
                        (yBodyPosition[i], yBodyPosition[i - 1]) = (yBodyPosition[i - 1], yBodyPosition[i]);
                    }
                    
                    xCharacterPosition--;
                    WriteAt(_characterBody, xBodyPosition[0]  , yBodyPosition[0] );
                    WriteAt(_characterHead[0], xCharacterPosition, yCharacterPosition);
                    break;
                case SnakeDirection.Up:
                    WriteAt(' ', tempBodyX, tempBodyY);
                    
                    xBodyPosition[snakeLenght - 1] = xCharacterPosition;
                    yBodyPosition[snakeLenght - 1] = yCharacterPosition;
                    
                    for (int i = snakeLenght - 1; i > 0; i--)
                    {
                        (xBodyPosition[i], xBodyPosition[i - 1]) = (xBodyPosition[i - 1], xBodyPosition[i]);
                        (yBodyPosition[i], yBodyPosition[i - 1]) = (yBodyPosition[i - 1], yBodyPosition[i]);
                    }
                    
                    yCharacterPosition--;
                    WriteAt(_characterBody, xBodyPosition[0]  , yBodyPosition[0] );
                    WriteAt(_characterHead[0], xCharacterPosition, yCharacterPosition);
                    break;
                case SnakeDirection.Down:
                    WriteAt(' ', tempBodyX, tempBodyY);
                    
                    xBodyPosition[snakeLenght - 1] = xCharacterPosition;
                    yBodyPosition[snakeLenght - 1] = yCharacterPosition;
                    
                    for (int i = snakeLenght - 1; i > 0; i--)
                    {
                        (xBodyPosition[i], xBodyPosition[i - 1]) = (xBodyPosition[i - 1], xBodyPosition[i]);
                        (yBodyPosition[i], yBodyPosition[i - 1]) = (yBodyPosition[i - 1], yBodyPosition[i]);
                    }
                    
                    yCharacterPosition++;
                    WriteAt(_characterBody, xBodyPosition[0]  , yBodyPosition[0] );
                    WriteAt(_characterHead[0], xCharacterPosition, yCharacterPosition);
                    break;
            }
            
            checkTheCollider(tempBodyX, tempBodyY);
            EndOfTheLine();
        }
        
    }

    //Checks if player bumps into a wall, tail or food.
    private void checkTheCollider(int tempBodyX, int tempBodyY)
    {
        if ((xCharacterPosition == xFoodPosition) & (yCharacterPosition == yFoodPosition))
        {
            FoodSpawn();
            MakeCharacterBigger(tempBodyX, tempBodyY);
        }
        if ((xCharacterPosition == 0) | (yCharacterPosition == 0) | (xCharacterPosition == _xBorderSize - 1) | (yCharacterPosition == _yBorderSize))
            endGame();

        for (int i = 0; i < snakeLenght; i++)
        {
            if (xBodyPosition[i] == xCharacterPosition & yBodyPosition[i] == yCharacterPosition)
                endGame();
        }
    }

    //Stops the thread with "You're Dead" text.
    void endGame()
    {
        string endingMessage = "You're Dead";
        WriteAtString(endingMessage, (_xBorderSize / 2) - (endingMessage.Length/2), _yBorderSize/2);
        EndOfTheLine();
        Thread.Sleep(Timeout.Infinite);
        Console.ReadKey();
    }

    //Reassigns the cursor to the bottom right after each operation.
    private void EndOfTheLine()
    {
        Console.SetCursorPosition(_xBorderSize,_yBorderSize); 
    }

}
