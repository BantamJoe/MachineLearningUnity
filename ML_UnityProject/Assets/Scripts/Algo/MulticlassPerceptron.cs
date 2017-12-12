using System.Collections.Generic;
using UnityEngine;
using MachineLearning.Scene;

namespace MachineLearning.Algo
{
    /// <summary>
    /// Un perceptron a deux couches a deux neurones en C#. 
    /// </summary>
    public class MulticlassPerceptron : MonoBehaviour
    {
        public bool auto;
        public bool useSmallestError;

        [Tooltip("Taux d'apprentissage")]
        public float a = 0.001f;

        int n;
        float[,] input;
        int[,] outputReal;
        float[,] outputFound;
        float[,] weights;

        int smallestError;
        float[,] betterWeights;

        List<int> misclassed;

        public int RunPerceptron(int iterations)
        {
            if (smallestError <= 0)
            {
                LogManager.Log("Error is already 0.");
                return 0;
            }

            Debug.Log("Running Perceptron.");

            int i = 0;
            for (; i < iterations; i++)
            {
                GetBetter();

                if (smallestError <= 0)
                {
                    break;
                }
            }

            LogManager.Log("Ran perceptron for " + i + " iterations. Error = " + smallestError + ".");
            return smallestError;
        }

        private void Init()
        {
            Entity[] entities = ObjectManager.Instance.GetComponentsInChildren<Entity>();

            n = entities.Length;

            input = new float[n, 2];
            outputReal = new int[n, 3];
            outputFound = new float[n, 3];
            weights = new float[3, 3];
            betterWeights = new float[3, 3];

            Vector2 pos;
            for (int i = 0; i < n; i++)
            {
                pos = entities[i].transform.position;
                input[i, 0] = pos.x;
                input[i, 1] = pos.y;
                int entityState = entities[i].State;
                switch (entityState)
                {
                case 1: // B
                    outputReal[i, 0] = 1;
                    outputReal[i, 1] = -1;
                    outputReal[i, 2] = -1;
                    break;
                case 2: // R
                    outputReal[i, 0] = -1;
                    outputReal[i, 1] = 1;
                    outputReal[i, 2] = -1;
                    break;
                case 3: // G
                    outputReal[i, 0] = -1;
                    outputReal[i, 1] = -1;
                    outputReal[i, 2] = 1;
                    break;
                default:
                    break;
                }
            }

            for (int i = 0; i < weights.GetLength(0); i++)
            {
                weights[i, 0] = Random.Range(-0.5f, 0.5f);
                weights[i, 1] = Random.Range(-0.5f, 0.5f);
                weights[i, 2] = Random.Range(-0.5f, 0.5f);
            }
            betterWeights = weights.Clone() as float[,];

            misclassed = new List<int>(n);
        }

        private void Start()
        {
            Init();

            ComputeMisclassifiedList();
            smallestError = misclassed.Count;
            BackgroundManager.Instance.PaintMultiClasses(weights);
        }

        private void Update()
        {
            if (auto)
            {
                GetBetter();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                auto = !auto;
            }
        }

        private void ComputeMisclassifiedList()
        {
            misclassed.Clear();

            float val;
            for (int i = 0; i < n; i++)
            {
                val = input[i, 0] * weights[0, 0] + input[i, 1] * weights[1, 0] + weights[2, 0];
                outputFound[i, 0] = Tanh(val);
                val = input[i, 0] * weights[0, 1] + input[i, 1] * weights[1, 1] + weights[2, 1];
                outputFound[i, 1] = Tanh(val);
                val = input[i, 0] * weights[0, 2] + input[i, 1] * weights[1, 2] + weights[2, 2];
                outputFound[i, 2] = Tanh(val);

                if (outputFound[i, 0] > 0 && outputReal[i, 0] < 0
                    || outputFound[i, 0] < 0 && outputReal[i, 0] > 0
                    || outputFound[i, 1] > 0 && outputReal[i, 1] < 0
                    || outputFound[i, 1] < 0 && outputReal[i, 1] > 0
                    || outputFound[i, 2] > 0 && outputReal[i, 2] < 0
                    || outputFound[i, 2] < 0 && outputReal[i, 2] > 0)
                {
                    misclassed.Add(i);
                }
            }

            if (useSmallestError)
            {
                Debug.Log("Error = " + misclassed.Count + " (better = " + smallestError + ")");
            }
            else
            {
                Debug.Log("Error = " + misclassed.Count);
            }
        }

        public void GetBetter()
        {
            if (misclassed.Count <= 0)
            {
                return;
            }

            int badOne = misclassed[Random.Range(0, misclassed.Count)];
            float diff1 = outputReal[badOne, 0] - outputFound[badOne, 0];
            float diff2 = outputReal[badOne, 1] - outputFound[badOne, 1];
            float diff3 = outputReal[badOne, 2] - outputFound[badOne, 2];

            weights[0, 0] += a * input[badOne, 0] * diff1;
            weights[1, 0] += a * input[badOne, 1] * diff1;
            weights[2, 0] += a * diff1;

            weights[0, 1] += a * input[badOne, 0] * diff2;
            weights[1, 1] += a * input[badOne, 1] * diff2;
            weights[2, 1] += a * diff2;

            weights[0, 2] += a * input[badOne, 0] * diff3;
            weights[1, 2] += a * input[badOne, 1] * diff3;
            weights[2, 2] += a * diff3;

            ComputeMisclassifiedList();

            if (useSmallestError)
            {
                if (misclassed.Count <= smallestError)
                {
                    betterWeights = weights.Clone() as float[,];
                    smallestError = misclassed.Count;
                    BackgroundManager.Instance.PaintMultiClasses(betterWeights);

                }
            }
            else
            {
                BackgroundManager.Instance.PaintMultiClasses(weights);
            }
        }

        private float Tanh(float val)
        {
            return (float)System.Math.Tanh(val);
        }
        private int Sign(float val)
        {
            return System.Math.Sign(val);
        }
    }
}