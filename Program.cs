using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Program
{
    private static GradientBoostingModel model = new GradientBoostingModel(); //Create a new instance of the model
    private static List<(string Team, double Actual, double Predicted)> predictions = new List<(string, double, double)>(); //Create the list for showing model training data

    public static void Main(string[] args) //Main menu instance
    {
        AnimatedTitle(); //Show animated title
        var menu = new Menu();
        menu.ShowMenu(OnMenuSelect);
    }

    private static void AnimatedTitle() //Smooth blinking title with centering
    {
        string[] titleLines = new string[]
        {
            "░▒▓███████▓▒░ ░▒▓████████▓▒ ░▒▓█▓▒░░▒▓█▓▒ ░▒▓████████▓▒ ░▒▓███████▓▒░  ▒▓█▓▒░░▒▓█▓▒ ░▒▓████████▓▒░       ░▒▓██████▓▒░ ░▒▓█▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░        ░▒▓█▓▒▒▓█▓▒░ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░ ",
            "░▒▓███████▓▒░ ░▒▓██████▓▒░   ░▒▓█▓▒▒▓█▓▒░ ░▒▓██████▓▒░  ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓██████▓▒░        ░▒▓████████▓▒ ░▒▓█▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░         ░▒▓█▓▓█▓▒░  ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░         ░▒▓█▓▓█▓▒░  ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒ ░▒▓████████▓▒    ░▒▓██▓▒░   ░▒▓████████▓▒ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓██████▓▒░ ░▒▓████████▓▒░      ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░ ",
            "May the sales be ever in your favor..."
        };

        for (int blink = 0; blink < 3; blink++) //Repeat blinking 3 times
        {
            Console.Clear();

            foreach (string line in titleLines)
            {
                CenterText(line); //Center each line of ASCII art
            }

            System.Threading.Thread.Sleep(300); //Pause for blinking effect

            Console.Clear(); //Blink effect by clearing
            System.Threading.Thread.Sleep(300); //Pause for off effect
        }

        Console.Clear(); //Ensure title stays after blinking
        foreach (string line in titleLines)
        {
            CenterText(line); //Display centered title without blinking
        }
    }
    private static void StaticTitle(){ //Centered Static Title with tagline
        

        string text = "░▒▓███████▓▒░ ░▒▓████████▓▒ ░▒▓█▓▒░░▒▓█▓▒ ░▒▓████████▓▒ ░▒▓███████▓▒░  ▒▓█▓▒░░▒▓█▓▒ ░▒▓████████▓▒░       ░▒▓██████▓▒░ ░▒▓█▓▒░\n░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░\n░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░        ░▒▓█▓▒▒▓█▓▒░ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░\n░▒▓███████▓▒░ ░▒▓██████▓▒░   ░▒▓█▓▒▒▓█▓▒░ ░▒▓██████▓▒░  ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓██████▓▒░        ░▒▓████████▓▒ ░▒▓█▓▒░\n░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░         ░▒▓█▓▓█▓▒░  ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░\n░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░         ░▒▓█▓▓█▓▒░  ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░             ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░\n░▒▓█▓▒░░▒▓█▓▒ ░▒▓████████▓▒    ░▒▓██▓▒░   ░▒▓████████▓▒ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓██████▓▒░ ░▒▓████████▓▒░      ░▒▓█▓▒░░▒▓█▓▒ ░▒▓█▓▒░\nMay the sales be ever in your favor..."; //"It would be really funny if you made a line over 1000 characters long" He thought to himself.
        foreach (string line in text.Split('\n')){
            int logoPadding = (Console.WindowWidth - line.Length)/2;
            System.Console.WriteLine(new string(' ', Math.Max(0, logoPadding))+line); //It would also be funny if we rewrote the centertext system command
        }
    } 
    private static void CenterText(string text) //Centers text dynamically based on the console width
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int padding = (windowWidth - textLength) / 2;

        if (padding > 0) Console.Write(new string(' ', padding)); //Add padding spaces
        Console.WriteLine(text);
    }

    private static void PrintWithTypingEffect(string text, int delay = 18) //Print text one character at a time for dramatic effect
    {
        foreach (char c in text)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    private static void ShowLoading(string message, int length = 20, int delay = 20) //Simulates a loading effect
    {
        Console.Write(message + " [");
        for (int i = 0; i < length; i++)
        {
            Console.Write("#");
            System.Threading.Thread.Sleep(delay);
        }
        PrintWithTypingEffect("]");
    }

    private static void OnMenuSelect(int option) //Menu Routing
    {
        switch (option)
        {
            case 0:
                ShowAbout();
                break;
                
            case 1:
                ShowLoading("Loading Prediction Engine...");
                RunPrediction();
                break;

            case 2:
                ShowLoading("Gathering Predictions...");
                ShowAllPredictions();
                break;
                
            case 3:
                ShowLoading("Calculating Errors...");
                ShowLastError();
                break;
                
            case 4:
                ShowLoading("Preparing Next Prediction...");
                PredictNextGame();
                break;
                
            case 5:
                Console.Clear();
                StaticTitle();
                PrintWithTypingEffect("Exiting program... Goodbye!", 20);
                Environment.Exit(0);
                break;
        }
    }

    private static void PredictNextGame()
    {
        Console.Clear();
        PrintWithTypingEffect("=== Predict Revenue for the Next Game ===");

        Console.Write("Enter if the game is home or away (home/away): ");
        string homeAway = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Enter the opposing team: ");
        string team = Console.ReadLine()?.Trim();

        double spread;
        do
        {
            Console.Write("Enter the spread (e.g., -10.5): ");
            if (!double.TryParse(Console.ReadLine(), out spread) || spread > -5 || spread < -100)
            {
                Console.WriteLine("Invalid input. Spread must be a number between -5 and -100.");
                Console.WriteLine("Spread represents the number of points the winning team is projected to win by.");
            }
            else
            {
                break;
            }
        } while (true);

        PrintWithTypingEffect($"Encoded Home/Away: {homeAway} => {GameDayData.EncodeHomeAway(homeAway)}");
        PrintWithTypingEffect($"Encoded Team: {team} => {GameDayData.EncodeTeam(team)}");

        int homeAwayEncoded = GameDayData.EncodeHomeAway(homeAway);
        int teamEncoded = GameDayData.EncodeTeam(team);

        if (homeAwayEncoded == -1 || teamEncoded == -1)
        {
            PrintWithTypingEffect("Invalid inputs. Ensure the Home/Away status and Team are correctly inputted.");
            PrintWithTypingEffect($"Valid Home/Away values: {string.Join(", ", GameDayData.HomeAwayEncoding.Keys)}");
            PrintWithTypingEffect($"Valid Teams: {string.Join(", ", GameDayData.TeamEncoding.Keys)}");
            PrintWithTypingEffect("Press any key to return...");
            Console.ReadKey();
            return;
        }

        double[] inputFeatures = { homeAwayEncoded, teamEncoded, spread };

        double predictedRevenue = model.Predict(inputFeatures);
        PrintWithTypingEffect($"\nPredicted Revenue for the Next Game: ${predictedRevenue:0.00}");
        PrintWithTypingEffect($"Predicted Error for this result: {Math.Sqrt(model.LastError):0.00}");
        PrintWithTypingEffect("Press any key to return...");
        Console.ReadKey();
    }

    private static void RunPrediction() //User must actually run the prediction for all previous games in order to predict any.
    {
        Console.Clear();
        PrintWithTypingEffect("=== Run Prediction ===");

        var dataSet = GameDayData.LoadDataset("gameday-data.csv"); //Load dataset and debug encodings
        GameDayData.DebugEncodings(); //Output encoding mappings for validation

        var (features, labels) = GameDayData.PrepareData(dataSet); //Prepare features and labels

        model = new GradientBoostingModel(); //Create a new model and train the model
        model.Train(features, labels);

        predictions.Clear(); //Generate predictions based on information gathered from all games
        for (int i = 0; i < features.Length; i++)
        {
            double predicted = model.Predict(features[i]);
            predictions.Add((dataSet[i].Team, labels[i], predicted));
        }

        PrintWithTypingEffect("Training complete. Predictions are now available.");
        PrintWithTypingEffect("Press any key to return to the menu...");
        Console.ReadKey();
    }

    private static void ShowLastError()
    {
        Console.Clear();
        PrintWithTypingEffect("=== Last Training Error ===");
        PrintWithTypingEffect($"Mean Squared Error (MSE): {model.LastError:0.00}");
        PrintWithTypingEffect($"Next predicted error: {Math.Sqrt(model.LastError):0.00}"); //Root of the MSE or outputs 0 if no data has been trained
        PrintWithTypingEffect("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void ShowAllPredictions()
    {
        Console.Clear();
        PrintWithTypingEffect("=== All Predictions ===");
        PrintWithTypingEffect($"{"Team",-20} {"Actual Revenue",-15} {"Predicted Revenue",-20}"); //Align columns in the console
        PrintWithTypingEffect(new string('-', 55)); //Prints 55 hyphens

        foreach (var (team, actual, predicted) in predictions) //Cycles through all the predictions made after training
        {
            Console.WriteLine($"{team,-20} {actual,-15:0.00} {predicted,-20:0.00}"); //Aligns outputs with headers above
        }

        Console.WriteLine();
        PrintWithTypingEffect(new string('-', 55));
        PrintWithTypingEffect("This is a table. Scroll up to see any specific prediction.");
        PrintWithTypingEffect("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void ShowAbout()
    {
        Console.Clear();
        StaticTitle();
        System.Threading.Thread.Sleep(500); //Pauses after the ASCII art populates and waits half a second for smooth transition to typing.
        PrintWithTypingEffect("\n\n=====About=====",8);
        PrintWithTypingEffect("This application predicts game-day revenue using Gradient Boosting.",15);
        PrintWithTypingEffect("Gradient Boosting is a form of machine learning which takes a single user-inputted prediction, and modifies that prediction over many training cycles called epochs.",15);
        PrintWithTypingEffect("The original prediction of sales in this model is $0. However, due to a large amount of training cycles, it is capable of a high level of accuracy regardless of the original prediction.",15);
        Console.WriteLine();
        PrintWithTypingEffect("=====How to use this application=====",8);
        PrintWithTypingEffect("This model is trained on historical data including Home/Away status, Opponent Team, and Betting Spread",15);
        PrintWithTypingEffect("(This program recognizes Betting Spread as how many points the winning team expects to win by, represented by a negative number between -4 and -100)",15);
        PrintWithTypingEffect("Predictions cannot be made without first training the data. You must select the second menu option before reviewing any training results or attempting to make predictions.",15);
        PrintWithTypingEffect("\n\nPress any key to return to the menu...",8);
        Console.ReadKey();
    }
}