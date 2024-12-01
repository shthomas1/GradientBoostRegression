using System;
using System.Collections.Generic;

public class Program
{
    private static GradientBoostingModel model = new GradientBoostingModel(); //Create a new instance of the model
    private static List<(string Team, double Actual, double Predicted)> predictions = new List<(string, double, double)>(); //Create the list for showing model training data

    public static void Main(string[] args) //Main menu instance
    {
        var menu = new Menu();
        menu.ShowMenu(OnMenuSelect);
    }

    private static void OnMenuSelect(int option) //Menu Routing
    {
        switch (option)
        {
            case 0:
                RunPrediction();
                break;
            case 1:
                ShowAllPredictions();
                break;
            case 2:
                ShowLastError();
                break;
            case 3:
                PredictNextGame();
                break;
            case 4:
                ShowAbout();
                break;
            case 5:
                Environment.Exit(0);
                break;
        }
    }

    private static void ShowAbout()
    {
        Console.Clear();
        Console.WriteLine("=== About ===");
        Console.WriteLine("This application predicts game-day revenue using Gradient Boosting.");
        Console.WriteLine("The model is trained on historical data including Home/Away status, Opponent Team, and Betting Spread \n(This program recognizes Betting Spread as how many points the winning team expects to win by, represented by a negative number.\n)");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }

    private static void RunPrediction() //User must actually run the prediction for all previous games in order to predict any.
    {
        Console.Clear();
        Console.WriteLine("=== Run Prediction ===");

        // Load dataset and debug encodings
        var dataSet = GameDayData.LoadDataset("gameday-data.csv");
        GameDayData.DebugEncodings(); //Output encoding mappings for validation - When things go wrong, we know what went wrong.

        //Prepare features and labels
        var (features, labels) = GameDayData.PrepareData(dataSet);

        //Create a new model and train the model
        model = new GradientBoostingModel();
        model.Train(features, labels);

        //Generate predictions based on information gathered from all games.
        predictions.Clear();
        for (int i = 0; i < features.Length; i++)
        {
            double predicted = model.Predict(features[i]);
            predictions.Add((dataSet[i].Team, labels[i], predicted));
        }

        Console.WriteLine("Training complete. Predictions are now available.");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }

    private static void ShowLastError()
    {
        Console.Clear();
        Console.WriteLine("=== Last Training Error ===");
        Console.WriteLine($"Mean Squared Error (MSE): {model.LastError:0.00}");
        Console.WriteLine($"Predicted error: {Math.Sqrt(model.LastError):0.00}"); //Just takes the root of the MSE, or outputs 0 if no data has been trained.
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void ShowAllPredictions()
    {
        Console.Clear();
        Console.WriteLine("=== All Predictions ===");
        Console.WriteLine($"{"Team",-20} {"Actual Revenue",-15} {"Predicted Revenue",-20}"); //The -20, -15, 55 and ,-20 in this row and all subsequent rows are related to the alignment of the columns in the console.
        Console.WriteLine(new string('-', 55));

        foreach (var (team, actual, predicted) in predictions) //Cycles through all the predictions made after the final epoch of training.
        {
            Console.WriteLine($"{team,-20} {actual,-15:0.00} {predicted,-20:0.00}"); //Aligns all the outputs of the actual values with their labels above, as described above.
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void PredictNextGame()
    {
        Console.Clear();
        Console.WriteLine("=== Predict Revenue for the Next Game ===");

        //Get user input for Home/Away status
        Console.Write("Enter if the game is home or away (home/away): ");
        string homeAway = Console.ReadLine()?.Trim().ToLower();

        //Get user input for Opponent Team
        Console.Write("Enter the opposing team: ");
        string team = Console.ReadLine()?.Trim();

        //Get user input for Spread
        Console.Write("Enter the spread (e.g., -10.5): ");
        double spread;
        while (!double.TryParse(Console.ReadLine(), out spread))
        {
            Console.Write("Invalid input. Please enter a valid spread (e.g., -10.5): ");
        }

        //Log encoding results for debugging
        Console.WriteLine($"Encoded Home/Away: {homeAway} => {GameDayData.EncodeHomeAway(homeAway)}");
        Console.WriteLine($"Encoded Team: {team} => {GameDayData.EncodeTeam(team)}");

        //Validate inputs
        int homeAwayEncoded = GameDayData.EncodeHomeAway(homeAway);
        int teamEncoded = GameDayData.EncodeTeam(team);

        if (homeAwayEncoded == -1 || teamEncoded == -1)
        {
            Console.WriteLine("Invalid inputs. Ensure the Home/Away status and Team are correctly inputted (Not Case Sensitive).");
            Console.WriteLine($"Valid Home/Away values: {string.Join(", ", GameDayData.HomeAwayEncoding.Keys)}");
            Console.WriteLine($"Valid Teams: {string.Join(", ", GameDayData.TeamEncoding.Keys)}");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        //Prepare features for prediction
        double[] inputFeatures = new double[]
        {
            homeAwayEncoded,
            teamEncoded,
            spread
        };

        //Perform prediction and output to console
        double predictedRevenue = model.Predict(inputFeatures);
        Console.WriteLine($"\nPredicted Revenue for the Next Game: ${predictedRevenue:0.00}");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }
}
