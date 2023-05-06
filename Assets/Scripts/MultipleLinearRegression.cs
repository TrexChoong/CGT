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

    public void LoadDataFromCSV(string filePath)
    {
        Debug.Log("Trigger file start");
        StreamReader reader = new StreamReader(filePath);
        Debug.Log("Trigger file read");
        while (!reader.EndOfStream)
        {
            string[] line = reader.ReadLine().Split(',');
            Debug.Log("Trigger row input raw: " + line[0]);
            Debug.Log("Trigger row input raw: " + line[1]);
            Debug.Log("Trigger row input raw: " + line[2]);
            Debug.Log("Trigger row input raw: " + line[3]);
            float[] x = new float[line.Length - 1];
            Debug.Log("Trigger row size: " + line.Length);
            //for (int i = 0; i < line.Length - 1; i++)
            for (int i = 0; i < line.Length - 1; i++)
            
            {
                Debug.Log("Trigger file input: " + line[i] +" "+ i);
                x[i] = float.Parse(line[i]);
            }
            float y = float.Parse(line[line.Length - 1]);
            xValues.Add(x);
            yValues.Add(y);
        }
        Debug.Log("Trigger file process");
        CalculateCoefficients(xValues, yValues);
    }

    public void CalculateCoefficients(List<float[]> xValues, List<float> yValues)
    {
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
        Debug.Log("CALCULATE COEFFICIENTS");
        Debug.Log("x1 = " + x1[1]);
        Debug.Log("x2 = " + x2[1]);
        Debug.Log("x3 = " + x3[1]);
        Debug.Log("Y = " + y[1]);
        Debug.Log("b1 = " + coefficientArray[0]);
        Debug.Log("b2 = " + coefficientArray[1]);
        Debug.Log("b3 = " + coefficientArray[2]);
        Debug.Log("a = " + coefficientArray[3]);
        Debug.Log("Mean of Y = " + meanY);
    }

    //public void CalculateCoefficients(List<float[]> xValues, List<float> yValues)
    //{
    //    // int numberOfDataPoints = yValues.Count;
    //    // int numberOfIndependentVariables = xValues[0].Length;
    //    // Matrix4x4 xMatrix = new Matrix4x4();
    //    // Vector4 yVector = new Vector4();

    //    // Debug.Log("Trigger calculate start");
    //    // // Fill the xMatrix and yVector with the provided values
    //    // for (int i = 0; i < numberOfDataPoints; i++)
    //    // {
    //    // Debug.Log("Trigger calculate loop a" );
    //    // //     for (int j = 0; j < numberOfIndependentVariables; j++)
    //    // //     {
    //    // // Debug.Log("Trigger calculate loop b " + i);
    //    // // Debug.Log("TEST VALUE " + i);
    //    // //         //xMatrix[i, j] = xValues[i, j];
    //    // //     }

    //    // // Debug.Log("TEST VALUE " + xValues[0][0]);
    //    // // Debug.Log("TEST VALUE 2" + xValues[0][1]);
    //    // // Debug.Log("TEST VALUE 3" + xValues[0][2]);
    //    // // Debug.Log("TEST VALUE 4" + xValues[0][3]);
    //    // Vector4 container = new Vector4(xValues[i][0],xValues[i][1],xValues[i][2], yValues[i]);
    //    // xMatrix.SetRow(i,container);
    //    // Debug.Log("Trigger loop :"+ i);
    //    //     xMatrix[i, numberOfIndependentVariables] = 1;
    //    // Debug.Log("Trigger loop :"+ i);
    //    //     yVector[i] = yValues[i];
    //    // }

    //    // // Calculate the coefficients using matrix algebra
    //    // Matrix4x4 xMatrixTranspose = xMatrix.transpose;
    //    // Debug.Log("Trigger calculate a:" + xMatrixTranspose.ToString());
    //    // Matrix4x4 xMatrixInverse = xMatrixTranspose * xMatrix;
    //    // Debug.Log("Trigger calculate b:" + xMatrixInverse.ToString());
    //    // Vector4 xTy = xMatrixTranspose * yVector;
    //    // Debug.Log("Trigger calculate c:" + xTy.ToString());
    //    // Vector4 coefficients = xMatrixInverse.inverse * xTy;
    //    // Debug.Log("Trigger calculate d:" + coefficients.ToString());

    //    // // Convert the coefficients to float array and return
    //    // float[] coefficientsArray = { coefficients.x, coefficients.y, coefficients.z, coefficients.w };
    //    // m_coefficients = coefficientsArray;
    //    // return coefficientsArray;
    //    Debug.Log("CALCULATE COEFFICIENTS");
    //}

    public float Predict(float[] x)
    {
        float y = 0;
        if(m_coefficients.Length>0){
            y = m_coefficients[0];
            for (int i = 0; i < x.Length; i++)
            {
                y += m_coefficients[i + 1] * x[i];
            }
        }
        return y;
    }

    public void CheckTest()
    {
        Debug.Log("Successful call");
    }
}