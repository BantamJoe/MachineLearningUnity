using System.Collections.Generic;
using UnityEngine;
using MachineLearning.Scene;

namespace MachineLearning.Algo
{
    /// <summary>
    /// Un perceptron a deux couches a deux neurones en C#. 
    /// </summary>
    public class DualLayerPerceptron : MonoBehaviour
    {
        public bool auto;
        public bool useSmallestError;

        [Tooltip("Taux d'apprentissage")]
        public float a = 0.001f;

        int n;
        float[,] input;
        float[,] miPut;
        int[] outputReal;
        float[] outputFound;
        float[] weights;
        /* Couche 1
         * w0,0  w1,0  wb,0
         * w0,1  w1,1  wb,1
         * Couche 2
         * w0,0  w1,0  wb,1   */ 

        int smallestError;
        float[] betterWeights;

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
            outputReal = new int[n];
            outputFound = new float[n];
            miPut = new float[n, 2];
            weights = new float[(2+1)*2 + 2+1];
            betterWeights = new float[(2+1)*2 + 2+1];

            Vector2 pos;
            for (int i = 0; i < n; i++)
            {
                pos = entities[i].transform.position;
                input[i, 0] = pos.x;
                input[i, 1] = pos.y;
                outputReal[i] = (entities[i].State == 1) ? 1 : -1;
            }

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = UnityEngine.Random.Range(-0.5f, 0.5f);
            }
            betterWeights = weights.Clone() as float[];

            misclassed = new List<int>(n);
        }

        private void Start()
        {
            Init();

            ComputeMisclassifiedList();
            smallestError = misclassed.Count;
            BackgroundManager.Instance.Paint2Layer(weights);
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
                val = input[i, 0] * weights[0] + input[i, 1] * weights[1] + weights[2];
                miPut[i, 0] = Tanh(val);
                val = input[i, 0] * weights[3] + input[i, 1] * weights[4] + weights[5];
                miPut[i, 1] = Tanh(val);
                
                val = miPut[i, 0] * weights[6] + miPut[i, 1] * weights[7] + weights[8];
                outputFound[i] = Tanh(val);
                //outputFound[i] = Sign(val);

                if (outputFound[i] > 0 && outputReal[i] < 0
                    || outputFound[i] < 0 && outputReal[i] > 0)
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

            //// Retropropagation :

            // Last Layer
            float gradf = (1 - outputFound[badOne] * outputFound[badOne]) * (outputFound[badOne] - outputReal[badOne]);

            float grad0 = (1 - miPut[badOne, 0] * miPut[badOne, 0]) * (weights[6]) * gradf;
            float grad1 = (1 - miPut[badOne, 1] * miPut[badOne, 1]) * (weights[7]) * gradf;

            weights[0] -= a * input[badOne, 0] * grad0;
            weights[1] -= a * input[badOne, 1] * grad0;
            weights[2] -= a * grad0;

            weights[3] -= a * input[badOne, 0] * grad1;
            weights[4] -= a * input[badOne, 1] * grad1;
            weights[5] -= a * grad1;

            weights[6] -= a * miPut[badOne, 0] * gradf;
            weights[7] -= a * miPut[badOne, 1] * gradf;
            weights[8] -= a * gradf;

            //// Last layer
            //float gradientLocal = diff * outputFound[badOne] * (1 - outputFound[badOne]);
            ////Debug.Log("diff(" + diff + ") * found(" + outputFound[badOne] + ") * (1 - found("+ outputFound[badOne] + ")) = " + gradientLocal);
            //weights[1, 0, 0] += a * gradientLocal * miPut[badOne, 0];
            //weights[1, 1, 0] += a * gradientLocal * miPut[badOne, 1];
            //weights[1, 2, 0] += a * gradientLocal * miPut[badOne, 2];

            //// neurone 0
            //float gradientLocal0 = miPut[badOne, 0] * (1 - miPut[badOne, 0]) * (gradientLocal * weights[1, 0, 0]);
            //weights[0, 0, 0] += a * gradientLocal0 * input[badOne, 0];
            //weights[0, 0, 1] += a * gradientLocal0 * input[badOne, 1];
            //weights[0, 0, 2] += a * gradientLocal0;

            //// neurone 1
            //float gradientLocal1 = miPut[badOne, 1] * (1 - miPut[badOne, 1]) * (gradientLocal * weights[1, 1, 0]);
            //weights[0, 1, 0] += a * gradientLocal1 * input[badOne, 0];
            //weights[0, 1, 1] += a * gradientLocal1 * input[badOne, 1];
            //weights[0, 1, 2] += a * gradientLocal1;

            //// neurone 2
            //float gradientLocal2 = miPut[badOne, 2] * (1 - miPut[badOne, 2]) * (gradientLocal * weights[1, 2, 0]);
            //weights[0, 2, 0] += a * gradientLocal2 * input[badOne, 0];
            //weights[0, 2, 1] += a * gradientLocal2 * input[badOne, 1];
            //weights[0, 2, 2] += a * gradientLocal2;

            ComputeMisclassifiedList();

            if (useSmallestError)
            {
                if (misclassed.Count <= smallestError)
                {
                    betterWeights = weights.Clone() as float[];
                    smallestError = misclassed.Count;
                    BackgroundManager.Instance.Paint2Layer(betterWeights);

                }
            }
            else
            {
                BackgroundManager.Instance.Paint2Layer(weights);
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