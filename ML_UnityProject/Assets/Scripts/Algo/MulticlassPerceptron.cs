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
        int c; // classes
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
            
            for (int i = 0; i < n; i++)
            {
                c = Mathf.Max(c, entities[i].State);
            }
            Debug.Log("There are " + c + " classes");
            outputReal = new int[n, c];
            outputFound = new float[n, c];
            weights = new float[3, c];
            betterWeights = new float[3, c];

            Vector2 pos;
            for (int i = 0; i < n; i++)
            {
                int entityState = entities[i].State;
                pos = entities[i].transform.position;
                input[i, 0] = pos.x;
                input[i, 1] = pos.y;

                for (int d = 0; d < c; d++)
                {
                    outputReal[i, d] = d == entityState - 1 ? 1 : -1;
                }
            }

            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = Random.Range(-0.5f, 0.5f);
                }
            }
            betterWeights = weights.Clone() as float[,];

            misclassed = new List<int>(n);
        }

        private void Start()
        {
            Init();

            ComputeMisclassifiedList();
            smallestError = misclassed.Count;
            BackgroundManager.Instance.PaintMulticlass(weights);
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
                for (int d = 0; d < c; d++)
                {
                    val = input[i, 0] * weights[0, d] + input[i, 1] * weights[1, d] + weights[2, d];
                    outputFound[i, d] = Tanh(val);
                }

                for (int d = 0; d < c; d++)
                {
                    if (outputFound[i, d] > 0 && outputReal[i, d] < 0
                    || outputFound[i, d] < 0 && outputReal[i, d] > 0)
                    {
                        misclassed.Add(i);
                        break;
                    }
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

            for (int d = 0; d < c; d++)
            {
                float diff = outputReal[badOne, d] - outputFound[badOne, d];

                weights[0, d] += a * input[badOne, 0] * diff;
                weights[1, d] += a * input[badOne, 1] * diff;
                weights[2, d] += a * diff;
            }
            
            ComputeMisclassifiedList();

            if (useSmallestError)
            {
                if (misclassed.Count <= smallestError)
                {
                    betterWeights = weights.Clone() as float[,];
                    smallestError = misclassed.Count;
                    BackgroundManager.Instance.PaintMulticlass(betterWeights);

                }
            }
            else
            {
                BackgroundManager.Instance.PaintMulticlass(weights);
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