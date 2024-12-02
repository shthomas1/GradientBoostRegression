public class Menu
{
    private List<string> options;
    private int currentSelection;

    public Menu()
    {
        options = new List<string> //The list is the menu. The menu is toggled through by the arrow keys and selected with enter
        {
            "About",
            "Run All Predictions",
            "View All Previous Predictions",
            "Review Total Error on all Predictions",
            "Predict Next Game's Revenue",
            "Exit"
        };
        currentSelection = 0;
    }

    public void ShowMenu(Action<int> onSelect) //Handles the menu selection
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("=== Main Menu ===");

            for (int i = 0; i < options.Count; i++)
            {
                if (i == currentSelection)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {options[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"  {options[i]}");
                }
            }
            Console.ResetColor();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    currentSelection = (currentSelection > 0) ? currentSelection - 1 : options.Count - 1; //Toggles through the list (Reverse)
                    break;
                case ConsoleKey.DownArrow:
                    currentSelection = (currentSelection < options.Count - 1) ? currentSelection + 1 : 0; //Toggles through the list (Forward)
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    onSelect(currentSelection);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
