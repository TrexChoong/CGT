using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class MultipleLinearRegression : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<float[]> xValues = new List<float[]>();
    public List<float> yValues = new List<float>();
    public float[] m_coefficients;

    private double[] theta; // Parameters learned by the model
    public List<double[]> xAlter = new List<double[]>();
    public List<double> yAlter = new List<double>();
    public void LoadDataFromCSV(string filePath)
    {
        Debug.Log("Trigger file start");
        StreamReader reader = new StreamReader(filePath);
        Debug.Log("Trigger file read");
        while (!reader.EndOfStream)
        {
            string[] line = reader.ReadLine().Split(',');
            // Debug.Log("Trigger row input raw: " + line[0]);
            // Debug.Log("Trigger row input raw: " + line[1]);
            // Debug.Log("Trigger row input raw: " + line[2]);
            // Debug.Log("Trigger row input raw: " + line[3]);
            float[] x = new float[line.Length - 1];
            double[] xAltercase = new double[line.Length-1];
            //Debug.Log("Trigger row size: " + line.Length);
            //for (int i = 0; i < line.Length - 1; i++)
            for (int i = 1; i < line.Length - 1; i++)
            
            {
                //Debug.Log("Trigger file input: " + line[i] +" "+ i);
                x[i] = float.Parse(line[i]);
                xAltercase[i] = double.Parse(line[i]);
            }
            float y = float.Parse(line[0]);
            float yAltercase = float.Parse(line[0]);
            xValues.Add(x);
            yValues.Add(y);

            xAlter.Add(xAltercase);
            yAlter.Add(yAltercase);
        }
        reader.Dispose();
        TrainModel(xAlter, yAlter);
    }

    public void CalculateCoefficients(List<float[]> xValues, List<float> yValues)
    {
        Debug.Log("check xvalues:" + xValues[0].ToString());
        Debug.Log("check xvalues1:" + xValues[1]);
        Debug.Log("check xvalues2:" + xValues[2]);
        // Calculate the means of the independent variables.
        //float[] x1 = xValues[0];
        //float[] x2 = xValues[1];
        //float[] x3 = xValues[2];

        float[] x1 = new float[xValues.Count];
        float[] x2 = new float[xValues.Count];
        float[] x3 = new float[xValues.Count];
        float[] y = new float[yValues.Count];

        for (int i = 0; i < xValues.Count; i++)
        {
            x1[i] = xValues[i][0];
            x2[i] = xValues[i][1];
            x3[i] = xValues[i][2];
        }

        for (int i = 0; i < yValues.Count; i++)
        {
            y[i] = yValues[i];
        }

        float meanX1 = x1.Average();
        float meanX2 = x2.Average();
        float meanX3 = x3.Average();

        // Calculate the means of the dependent variable.
        float meanY = y.Average();

        // Calculate the sum of the squares of the independent variables
        float sumX1Squared = x1.Select(x => x * x).Sum();
        float sumX2Squared = x2.Select(x => x * x).Sum();
        float sumX3Squared = x3.Select(x => x * x).Sum();
        float sumXSquares = sumX1Squared + sumX2Squared + sumX3Squared;


        float sumX1X2 = x1.Zip(x2, (x1Val, x2Val) => x1Val * x2Val).Sum();
        float sumX1X3 = x1.Zip(x3, (x1Val, x3Val) => x1Val * x3Val).Sum();
        float sumX2X3 = x2.Zip(x3, (x2Val, x3Val) => x2Val * x3Val).Sum();
        float sumXX = sumX1Squared + sumX2Squared + sumX3Squared + 2 * (sumX1X2 + sumX1X3 + sumX2X3);

        int n = y.Length;

        //float sumX1X2 = Enumerable.Range(0, x1.Length).Sum(i => x1[i] * x2[i]);
        //float sumX1X3 = Enumerable.Range(0, x1.Length).Sum(i => x1[i] * x3[i]);
        //float sumX2X3 = Enumerable.Range(0, x2.Length).Sum(i => x2[i] * x3[i]);

        // Calculate the sum of squares of the dependent variable
        float sumYSquared = y.Select(yi => yi * yi).Sum();

        // Calculate the sum of products between each independent variable and the dependent variable
        float sumX1Y = Enumerable.Range(0, x1.Length).Sum(i => x1[i] * y[i]);
        float sumX2Y = Enumerable.Range(0, x2.Length).Sum(i => x2[i] * y[i]);
        float sumX3Y = Enumerable.Range(0, x3.Length).Sum(i => x3[i] * y[i]);
        float sumXY = sumX1Y + sumX2Y + sumX3Y;

        // Calculate the coefficients of the regression equation
        float b1 = ((n * sumXY) - (sumXSquares * meanY) - (sumXX * meanX1)) / ((n * sumX1Squared) - (sumXX * meanX1) - (sumXSquares));
        float b2 = ((n * sumXY) - (sumXSquares * meanY) - (sumXX * meanX2)) / ((n * sumX2Squared) - (sumXX * meanX2) - (sumXSquares));
        float b3 = ((n * sumXY) - (sumXSquares * meanY) - (sumXX * meanX3)) / ((n * sumX3Squared) - (sumXX * meanX3) - (sumXSquares));


        //float b1 = (sumX2Squared * sumX1Y - sumX1X2 * sumX2Y) / (sumX1Squared * sumX2Squared - sumX1X2 * sumX1X2);
        //float b2 = (sumX1Squared * sumX2Y - sumX1X2 * sumX1Y) / (sumX1Squared * sumX2Squared - sumX1X2 * sumX1X2);
        //float b3 = (sumX1Squared * sumX2Y - sumX1X2 * sumX1Y) / (sumX1Squared * sumX2Squared - sumX1X2 * sumX1X2);
        float a = meanY - b1 * meanX1 - b2 * meanX2 - b3 * meanX3;

        float[] coefficientArray = { b1, b2, b3, a};
        m_coefficients = coefficientArray;
        // Debug.Log("CALCULATE COEFFICIENTS");
        // Debug.Log("x1 = " + x1[1]);
        // Debug.Log("x2 = " + x2[1]);
        // Debug.Log("x3 = " + x3[1]);
        // Debug.Log("Y = " + y[1]);
        // Debug.Log("b1 = " + coefficientArray[0]);
        // Debug.Log("b2 = " + coefficientArray[1]);
        // Debug.Log("b3 = " + coefficientArray[2]);
        // Debug.Log("a = " + coefficientArray[3]);
        // Debug.Log("Mean of Y = " + meanY);
    }

    // Trains the model on the given input data
    public void TrainModel(List<double[]> inputs, List<double> outputs, double learningRate = 0.01, int numIterations = 100)
    {
        int n = inputs.Count; // Number of data points
        int p = inputs[0].Length; // Number of input features

        // Add a column of 1s to the beginning of the input matrix for the intercept term
        double[][] X = new double[n][];
        for (int i = 0; i < n - 1; i++)
        {
            X[i] = new double[p + 1];
            X[i][0] = 1;
            for (int j = 0; j < p; j++)
            {
                X[i][j + 1] = inputs[i][j];
                //Debug.Log("input index = " + i +" "+ j);
            }
        }

        // Initialize the parameters
        theta = new double[p + 1];

        // Perform gradient descent
        for (int iter = 0; iter < numIterations; iter++)
        {
            // Compute the predicted outputs and errors
            double[] predictions = new double[n];
            double[] errors = new double[n];
            for (int i = 0; i < n - 10; i++)
            {
                double prediction = 0;
                for (int j = 0; j < p + 1; j++)
                {
                    prediction += X[i][j] * theta[j];
                }
                predictions[i] = prediction;
                errors[i] = prediction - outputs[i];
            }

            // Update the parameters
            double[] gradients = new double[p + 1];
            for (int j = 0; j < p + 1; j++)
            {
                double gradient = 0;
                for (int i = 0; i < n - 10; i++)
                {
                    gradient += errors[i] * X[i][j];
                }
                gradients[j] = gradient / (n - 10);
            }
            for (int j = 0; j < p + 1; j++)
            {
                theta[j] -= learningRate * gradients[j];
            }
        }
    }

    public float Predict(float[] x)
    {
        float y = 0;
        if(m_coefficients.Length>0){
            y = m_coefficients[3];
            for (int i = 0; i < x.Length; i++)
            {
                y += m_coefficients[i] * x[i];
            }
        }
        return y;
    }

    // Predicts the speed of the player given their score, coin, and distance
    public double PredictSpeed(double score, double coin, double distance)
    {
        // Add a 1 to the beginning of the input vector for the intercept term
        double[] x = new double[] { 1, score, coin, distance };

        // Compute the predicted output
        double prediction = 0;
        for (int j = 0; j < theta.Length; j++)
        {
            prediction += x[j] * theta[j];
        }

        return prediction;
    }

    public void CheckTest()
    {
        Debug.Log("Successful call");
    }
}