using System;
using System.Collections.Generic;

public class GradientBoostingModel
{
    private readonly int numberOfIterations;
    private readonly double learningRate;
    private readonly int maxDepth;

    private List<DecisionTree> trees;

    public double LastError { get; private set; } // Track the last calculated error

    public GradientBoostingModel(int numberOfIterations = 500, double learningRate = 0.05, int maxDepth = 4)
    {
        this.numberOfIterations = numberOfIterations;
        this.learningRate = learningRate;
        this.maxDepth = maxDepth;
        this.trees = new List<DecisionTree>();
    }

    public void Train(double[][] features, double[] labels)
    {
        double[] residuals = new double[labels.Length];

        for (int i = 0; i < labels.Length; i++)
        {
            residuals[i] = labels[i];
        }

        for (int iter = 0; iter < numberOfIterations; iter++)
        {
            DecisionTree tree = new DecisionTree(maxDepth);
            tree.Train(features, residuals);

            trees.Add(tree);

            for (int i = 0; i < residuals.Length; i++)
            {
                residuals[i] -= learningRate * tree.Predict(features[i]);
            }
        }

        LastError = CalculateMeanSquaredError(features, labels);
    }

    public double Predict(double[] features)
    {
        double prediction = 0.0;

        foreach (var tree in trees)
        {
            prediction += learningRate * tree.Predict(features);
        }

        return prediction;
    }

    private double CalculateMeanSquaredError(double[][] features, double[] labels)
    {
        double errorSum = 0.0;

        for (int i = 0; i < features.Length; i++)
        {
            double prediction = Predict(features[i]);
            errorSum += Math.Pow(labels[i] - prediction, 2);
        }

        return errorSum / features.Length;
    }
}