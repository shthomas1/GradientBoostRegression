using System;
using System.Collections.Generic;

public class GradientBoostingModel
{
    private readonly int epochs; //Number of boosting iterations (how many trees will be added to the model)
    private readonly double learningRate; //Step size for updating residuals in each iteration (How much the model adjusts its 'guess' each time it iterates)
    private readonly int maxDepth; //Maximum depth of each decision tree in the model (In Gradient Boosting, a tree represents the full iteration, and the depth is how many 'leaves' are on the tree. These leaves group similar data points and make a prediction based on those points.)

    private List<DecisionTree> trees; //List to store the ensemble of decision trees

    public double LastError { get; private set; } //Tracks the final Mean Squared Error (MSE) after training

    public GradientBoostingModel(int epochs = 500, double learningRate = 0.05, int maxDepth = 4)
    {
        this.epochs = epochs;
        this.learningRate = learningRate;
        this.maxDepth = maxDepth;
        this.trees = new List<DecisionTree>();
    }

    public void Train(double[][] features, double[] labels)
    {
        double[] residuals = new double[labels.Length]; //Array to store residuals for each data point

        for (int i = 0; i < labels.Length; i++)
        {
            residuals[i] = labels[i]; //Start with residuals equal to the actual labels (predictions are initially zero)
        }

        for (int iter = 0; iter < epochs; iter++) //Perform boosting for the specified number of epochs
        {
            DecisionTree tree = new DecisionTree(maxDepth);
            tree.Train(features, residuals); //Train the tree on the current residuals
            trees.Add(tree); //Add the trained tree to the ensemble

            for (int i = 0; i < residuals.Length; i++)
            {
                residuals[i] -= learningRate * tree.Predict(features[i]); //Update residuals using the tree's predictions
            }
        }

        LastError = CalculateMeanSquaredError(features, labels); //Calculate the final MSE after training
    }

    public double Predict(double[] features)
    {
        double prediction = 0.0; //The initial prediction is zero. This number does affect the final outcome by a small amount, but a large number of iterations makes this incredibly miniscule. In short, the user (In this case, the writer of this code, makes an initial prediction on sales for the first iteration across the board.)

        foreach (var tree in trees) //Aggregate predictions from all the trained trees
        {
            prediction += learningRate * tree.Predict(features); //Scale each tree's prediction by the learning rate
        }

        return prediction;
    }

    private double CalculateMeanSquaredError(double[][] features, double[] labels)
    {
        double errorSum = 0.0;

        for (int i = 0; i < features.Length; i++)
        {
            double prediction = Predict(features[i]); //Get the model's prediction for the current data point
            errorSum += Math.Pow(labels[i] - prediction, 2); //Compute the squared error
        }

        return errorSum / features.Length; //Return the average squared error (MSE)
    }
}
