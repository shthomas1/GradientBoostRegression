using System;
using System.Collections.Generic;

public class DecisionTree
{
    private int maxDepth;

    private class Node
    {
        public int FeatureIndex { get; set; }
        public double Threshold { get; set; }
        public double Prediction { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

    private Node root;

    public DecisionTree(int maxDepth)
    {
        this.maxDepth = maxDepth;
    }

    public void Train(double[][] features, double[] residuals)
    {
        root = BuildTree(features, residuals, 0);
    }

    private Node BuildTree(double[][] features, double[] residuals, int depth)
    {
        if (depth >= maxDepth || features.Length == 0)
        {
            return new Node
            {
                Prediction = CalculateMean(residuals)
            };
        }

        (int bestFeature, double bestThreshold) = FindBestSplit(features, residuals);

        if (bestFeature == -1)
        {
            return new Node
            {
                Prediction = CalculateMean(residuals)
            };
        }

        (double[][] leftFeatures, double[] leftResiduals, double[][] rightFeatures, double[] rightResiduals) =
            SplitData(features, residuals, bestFeature, bestThreshold);

        return new Node
        {
            FeatureIndex = bestFeature,
            Threshold = bestThreshold,
            Left = BuildTree(leftFeatures, leftResiduals, depth + 1),
            Right = BuildTree(rightFeatures, rightResiduals, depth + 1)
        };
    }

    public double Predict(double[] input)
    {
        return Predict(root, input);
    }

    private double Predict(Node node, double[] input)
    {
        if (node.Left == null && node.Right == null)
        {
            return node.Prediction;
        }

        if (input[node.FeatureIndex] <= node.Threshold)
        {
            return Predict(node.Left, input);
        }
        else
        {
            return Predict(node.Right, input);
        }
    }

    private (int, double) FindBestSplit(double[][] features, double[] residuals)
    {
        int bestFeature = -1;
        double bestThreshold = double.NaN;
        double bestError = double.MaxValue;

        for (int featureIndex = 0; featureIndex < features[0].Length; featureIndex++)
        {
            foreach (double threshold in GetUniqueValues(features, featureIndex))
            {
                double error = CalculateSplitError(features, residuals, featureIndex, threshold);

                if (error < bestError)
                {
                    bestError = error;
                    bestFeature = featureIndex;
                    bestThreshold = threshold;
                }
            }
        }

        return (bestFeature, bestThreshold);
    }

    private double CalculateSplitError(double[][] features, double[] residuals, int featureIndex, double threshold)
    {
        (double[][] leftFeatures, double[] leftResiduals, double[][] rightFeatures, double[] rightResiduals) =
            SplitData(features, residuals, featureIndex, threshold);

        double leftError = CalculateVariance(leftResiduals) * leftResiduals.Length;
        double rightError = CalculateVariance(rightResiduals) * rightResiduals.Length;

        return leftError + rightError;
    }

    private (double[][], double[], double[][], double[]) SplitData(double[][] features, double[] residuals, int featureIndex, double threshold)
    {
        List<double[]> leftFeatures = new List<double[]>();
        List<double[]> rightFeatures = new List<double[]>();
        List<double> leftResiduals = new List<double>();
        List<double> rightResiduals = new List<double>();

        for (int i = 0; i < features.Length; i++)
        {
            if (features[i][featureIndex] <= threshold)
            {
                leftFeatures.Add(features[i]);
                leftResiduals.Add(residuals[i]);
            }
            else
            {
                rightFeatures.Add(features[i]);
                rightResiduals.Add(residuals[i]);
            }
        }

        return (leftFeatures.ToArray(), leftResiduals.ToArray(), rightFeatures.ToArray(), rightResiduals.ToArray());
    }

    private double CalculateMean(double[] values)
    {
        double sum = 0.0;
        foreach (double value in values)
        {
            sum += value;
        }
        return sum / values.Length;
    }

    private double CalculateVariance(double[] values)
    {
        if (values.Length == 0) return 0;

        double mean = CalculateMean(values);
        double sumSquaredDifferences = 0.0;

        foreach (double value in values)
        {
            sumSquaredDifferences += (value - mean) * (value - mean);
        }

        return sumSquaredDifferences / values.Length;
    }

    private IEnumerable<double> GetUniqueValues(double[][] features, int featureIndex)
    {
        HashSet<double> uniqueValues = new HashSet<double>();

        foreach (double[] featureRow in features)
        {
            uniqueValues.Add(featureRow[featureIndex]);
        }

        return uniqueValues;
    }
}
