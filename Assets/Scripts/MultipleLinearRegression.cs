using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        // int numberOfDataPoints = yValues.Count;
        // int numberOfIndependentVariables = xValues[0].Length;
        // Matrix4x4 xMatrix = new Matrix4x4();
        // Vector4 yVector = new Vector4();

        // Debug.Log("Trigger calculate start");
        // // Fill the xMatrix and yVector with the provided values
        // for (int i = 0; i < numberOfDataPoints; i++)
        // {
        // Debug.Log("Trigger calculate loop a" );
        // //     for (int j = 0; j < numberOfIndependentVariables; j++)
        // //     {
        // // Debug.Log("Trigger calculate loop b " + i);
        // // Debug.Log("TEST VALUE " + i);
        // //         //xMatrix[i, j] = xValues[i, j];
        // //     }
        
        // // Debug.Log("TEST VALUE " + xValues[0][0]);
        // // Debug.Log("TEST VALUE 2" + xValues[0][1]);
        // // Debug.Log("TEST VALUE 3" + xValues[0][2]);
        // // Debug.Log("TEST VALUE 4" + xValues[0][3]);
        // Vector4 container = new Vector4(xValues[i][0],xValues[i][1],xValues[i][2], yValues[i]);
        // xMatrix.SetRow(i,container);
        // Debug.Log("Trigger loop :"+ i);
        //     xMatrix[i, numberOfIndependentVariables] = 1;
        // Debug.Log("Trigger loop :"+ i);
        //     yVector[i] = yValues[i];
        // }

        // // Calculate the coefficients using matrix algebra
        // Matrix4x4 xMatrixTranspose = xMatrix.transpose;
        // Debug.Log("Trigger calculate a:" + xMatrixTranspose.ToString());
        // Matrix4x4 xMatrixInverse = xMatrixTranspose * xMatrix;
        // Debug.Log("Trigger calculate b:" + xMatrixInverse.ToString());
        // Vector4 xTy = xMatrixTranspose * yVector;
        // Debug.Log("Trigger calculate c:" + xTy.ToString());
        // Vector4 coefficients = xMatrixInverse.inverse * xTy;
        // Debug.Log("Trigger calculate d:" + coefficients.ToString());

        // // Convert the coefficients to float array and return
        // float[] coefficientsArray = { coefficients.x, coefficients.y, coefficients.z, coefficients.w };
        // m_coefficients = coefficientsArray;
        // return coefficientsArray;
        Debug.Log("CALCULATE COEFFICIENTS");
    }

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
        Debug.Log("successful call");
    }
}