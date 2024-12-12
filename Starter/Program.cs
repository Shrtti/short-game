using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = {"('-')", "(^-^)", "(X_X)"};
string[] foods = {"@@@@@", "$$$$$", "#####"};

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit) 
{
     if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        shouldExit = true;
        break;
    }

    int speedAdjustment = ShouldAdjustSpeed() ? 3 : 0;
    
    
    Move(true, speedAdjustment);
}

// Returns true if the Terminal was resized 
bool TerminalResized() 
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood() 
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer() 
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer() 
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}


// Reads directional input from the Console and moves the player
void Move(bool allowTermination = false, int speedAdjustment = 0) 
{
    int delay = 100 - speedAdjustment;

    if (ShouldFreeze())
    {
        FreezePlayer(); 
        return;
    }

    int lastX = playerX;
    int lastY = playerY;
    
    ConsoleKey key = Console.ReadKey(true).Key;
    switch (key) 
    {
        case ConsoleKey.UpArrow:
            playerY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerY++; 
            break;
		case ConsoleKey.LeftArrow:  
            playerX--; 
            break;
		case ConsoleKey.RightArrow: 
            playerX++; 
            break;
		case ConsoleKey.Escape:     
            shouldExit = true; 
            break;
        default:
            if (allowTermination)
            {
                Console.Clear();
                Console.WriteLine("Nondirectional key pressed. Program exiting.");
                shouldExit = true;
            }
            return;
    }

    System.Threading.Thread.Sleep(delay);

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }


    playerX = Math.Clamp(playerX, 0, width);
    playerY = Math.Clamp(playerY, 0, height);


    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

    if (HasConsumedFood())
    {
        ChangePlayer();  // Update player's appearance
        ShowFood();      // Display new food
    }
}

// Clears the console, displays the food and player
void InitializeGame() 
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}

bool HasConsumedFood()
{
    // Check if the player's position matches the food's position
    return playerX == foodX && playerY == foodY;
}

bool ShouldFreeze()
{
    return player == states[2]; // states[2] corresponds to "(X_X)"
}

bool ShouldAdjustSpeed()
{
    return player == states[1]; // states[1] corresponds to "(^-^)"
}
