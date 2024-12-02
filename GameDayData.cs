using System;
using System.Collections.Generic;
using System.IO;

public class GameDayData
{
    public string HomeAway {get; set;}
    public string Team {get; set;}
    public double Spread {get; set;}
    public double Revenue {get; set;}

    private static Dictionary<string, int> homeAwayEncoding = new Dictionary<string, int>();
    private static Dictionary<string, int> teamEncoding = new Dictionary<string, int>();

    //Expose encodings as read-only properties
    public static IReadOnlyDictionary<string, int> HomeAwayEncoding => homeAwayEncoding;
    public static IReadOnlyDictionary<string, int> TeamEncoding => teamEncoding;

    public static List<GameDayData> LoadDataset(string filePath)
    {
        var dataSet = new List<GameDayData>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            reader.ReadLine(); //Skip header in csv file
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length != 4) continue;

                dataSet.Add(new GameDayData
                {
                    HomeAway = parts[0].Trim().ToLower(), //designates each position in the csv file
                    Team = parts[1].Trim().ToLower(),
                    Revenue = double.Parse(parts[2]),
                    Spread = double.Parse(parts[3])
                });
            }
        }

        GenerateEncodings(dataSet);
        return dataSet;
    }

    private static void GenerateEncodings(List<GameDayData> data)
    {
        homeAwayEncoding.Clear();
        teamEncoding.Clear();

        foreach (var item in data)
        {
            if (!homeAwayEncoding.ContainsKey(item.HomeAway))
                homeAwayEncoding[item.HomeAway] = homeAwayEncoding.Count;

            if (!teamEncoding.ContainsKey(item.Team))
                teamEncoding[item.Team] = teamEncoding.Count;
        }
    }

    public static int EncodeHomeAway(string homeAway)
    {
        return homeAwayEncoding.TryGetValue(homeAway.ToLower(), out int value) ? value : -1;
    }

    public static int EncodeTeam(string team)
    {
        return teamEncoding.TryGetValue(team.ToLower(), out int value) ? value : -1;
    }

    public static (double[][] Features, double[] Labels) PrepareData(List<GameDayData> data)
    {
        var features = new List<double[]>();
        var labels = new List<double>();

        foreach (var item in data)
        {
            features.Add(new double[]
            {
                EncodeHomeAway(item.HomeAway),
                EncodeTeam(item.Team),
                item.Spread
            });
            labels.Add(item.Revenue);
        }

        return (features.ToArray(), labels.ToArray());
    }

    public static void DebugEncodings()
    {
        Console.WriteLine("HomeAway Encoding:");
        foreach (var entry in HomeAwayEncoding)
        {
            Console.WriteLine($"  {entry.Key} => {entry.Value}");
        }

        Console.WriteLine("Team Encoding:");
        foreach (var entry in TeamEncoding)
        {
            Console.WriteLine($"  {entry.Key} => {entry.Value}");
        }
    }
}
