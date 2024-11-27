using System;
using System.Collections.Generic;

public class Menu
{
    private List<string> options;
    private int currentSelection;

    public Menu()
    {
        options = new List<string>
        {
            "Run All Predictions",
            "View All Previous Predictions",
            "Review Total Error on all Predictions",
            "Predict Next Game's Revenue",
            "About",
            "Exit"
        };
        currentSelection = 0;
    }

    public void ShowMenu(Action<int> onSelect)
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
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    currentSelection = (currentSelection > 0) ? currentSelection - 1 : options.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    currentSelection = (currentSelection < options.Count - 1) ? currentSelection + 1 : 0;
                    break;
                case ConsoleKey.Enter:
                    onSelect(currentSelection);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}