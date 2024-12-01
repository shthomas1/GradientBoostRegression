using System;
using System.Collections.Generic;

public class GradientBoostingModel
{
    private readonly int epochs; // Number of boosting iterations (how many trees will be added to the model)
    private readonly double learningRate; // Step size for updating residuals in each iteration
    private readonly int maxDepth; // Maximum depth of each decision tree in the model

    private List<DecisionTree> trees; // List to store the ensemble of decision trees

    public double LastError { get; private set; } // Tracks the final Mean Squared Error (MSE) after training

    public GradientBoostingModel(int epochs = 500, double learningRate = 0.05, int maxDepth = 4)
    {
        this.epochs = epochs;
        this.learningRate = learningRate;
        this.maxDepth = maxDepth;
        this.trees = new List<DecisionTree>();
    }

    public void Train(double[][] features, double[] labels)
    {
        if (features.Length == 0 || labels.Length == 0 || features.Length != labels.Length)
        {
            throw new ArgumentException("Features and labels must be non-empty and have the same length.");
        }

        double[] residuals = new double[labels.Length]; // Array to store residuals for each data point

        // Initialize residuals to the actual labels
        for (int i = 0; i < labels.Length; i++)
        {
            residuals[i] = labels[i];
        }

        for (int iter = 0; iter < epochs; iter++) // Perform boosting for the specified number of epochs
        {
            DecisionTree tree = new DecisionTree(maxDepth);
            tree.Train(features, residuals); // Train the tree on the current residuals
            trees.Add(tree); // Add the trained tree to the ensemble

            // Update residuals using the tree's predictions
            for (int i = 0; i < residuals.Length; i++)
            {
                double prediction = tree.Predict(features[i]);

                if (double.IsNaN(prediction) || double.IsInfinity(prediction))
                {
                    throw new InvalidOperationException($"Invalid prediction detected during training at iteration {iter}: {prediction}");
                }

                residuals[i] -= learningRate * prediction;
            }
        }

        LastError = CalculateMeanSquaredError(features, labels); // Calculate the final MSE after training

        if (double.IsNaN(LastError) || double.IsInfinity(LastError))
        {
            throw new InvalidOperationException("Invalid error calculated after training.");
        }
    }

    public double Predict(double[] features)
    {
        double prediction = 0.0; // Start with an initial prediction of zero

        foreach (var tree in trees) // Aggregate predictions from all the trained trees
        {
            double treePrediction = tree.Predict(features);

            if (double.IsNaN(treePrediction) || double.IsInfinity(treePrediction))
            {
                throw new InvalidOperationException($"Invalid tree prediction: {treePrediction}");
            }

            prediction += learningRate * treePrediction; // Scale each tree's prediction by the learning rate
        }

        return prediction;
    }

    private double CalculateMeanSquaredError(double[][] features, double[] labels)
    {
        double errorSum = 0.0;

        for (int i = 0; i < features.Length; i++)
        {
            double prediction = Predict(features[i]); // Get the model's prediction for the current data point

            if (double.IsNaN(prediction) || double.IsInfinity(prediction))
            {
                throw new InvalidOperationException($"Invalid prediction during error calculation: {prediction}");
            }

            errorSum += Math.Pow(labels[i] - prediction, 2); // Compute the squared error
        }

        return errorSum / features.Length; // Return the average squared error (MSE)
    }
}
