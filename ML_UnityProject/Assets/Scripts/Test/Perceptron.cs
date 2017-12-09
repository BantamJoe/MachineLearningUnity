using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test
{
    public class Perceptron : MonoBehaviour
    {
        public bool useSmallestError;

        [Tooltip("Taux d'apprentissage")]
        public float a = 0.05f;

        public BackgroundManager background;
        public ObjectManager objects;

        int n;
        int p = 2;
        float[,] input;
        int[] outputReal;
        int[] outputFound;
        float[] weights;

        int smallestError;
        float[] betterWeigts;

        List<int> misclassed;

        private void Init()
        {
            Entity[] entities = objects.GetComponentsInChildren<Entity>();

            n = entities.Length;

            input = new float[n, p];
            outputReal = new int[n];
            outputFound = new int[n];
            weights = new float[p + 1];
            betterWeigts = new float[p + 1];

            Vector2 pos;
            for (int i = 0; i < n; i++)
            {
                pos = entities[i].transform.position;
                input[i, 0] = pos.x;
                input[i, 1] = pos.y;
                outputReal[i] = entities[i].State - 1;
            }

            weights[0] = Random.Range(-1f, 1f);
            weights[1] = Random.Range(-1f, 1f);
            weights[2] = Random.Range(-1f, 1f);
            weights.CopyTo(betterWeigts, 0);

            misclassed = new List<int>(n);
        }

        private void Start()
        {
            Init();

            ComputeMisclassifiedList();
            smallestError = misclassed.Count;
            background.Paint(weights[0], weights[1], weights[2]);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
            {
                GetBetter();
            }
        }

        private void ComputeMisclassifiedList()
        {
            misclassed.Clear();

            for (int i = 0; i < n; i++)
            {
                float val = input[i, 0] * weights[0]
                    + input[i, 1] * weights[1] + weights[2];

                outputFound[i] = val == 0 ? -1 : val > 0 ? 1 : 0;

                if (outputFound[i] != outputReal[i])
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
            float diff = outputReal[badOne] - outputFound[badOne];
            weights[0] += a * input[badOne, 0] * diff;
            weights[1] += a * input[badOne, 1] * diff;
            weights[2] += a * diff;

            ComputeMisclassifiedList();

            if (useSmallestError)
            {
                if (misclassed.Count < smallestError)
                {
                    weights.CopyTo(betterWeigts, 0);
                    smallestError = misclassed.Count;
                }
                background.Paint(betterWeigts[0], betterWeigts[1], betterWeigts[2]);
            }
            else
            {
                background.Paint(weights[0], weights[1], weights[2]);
            }
        }
    }
}