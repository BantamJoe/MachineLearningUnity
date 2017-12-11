using System.Runtime.InteropServices;
using UnityEngine;
using MachineLearning.Scene;

namespace MachineLearning.Algo
{
    /// <summary>
    /// Un perceptron simple-couche qui utilise la librairie C++.
    /// </summary>
    public class SimplePerceptron : MonoBehaviour
    {
        #region DllImport
        [DllImport("machineLearning", EntryPoint = "CreatePerceptron")]
        public static extern void CreatePerceptron(float a, int n, int din);
        [DllImport("machineLearning", EntryPoint = "DestroyPerceptron")]
        public static extern void DestroyPerceptron();
        [DllImport("machineLearning", EntryPoint = "FillData")]
        public static extern void FillData(float[] input, float[] output);
        [DllImport("machineLearning", EntryPoint = "Run")]
        public static extern int Run(int iterations);
        [DllImport("machineLearning", EntryPoint = "GetOutput")]
        public static extern float GetOutput(float[] input);
        #endregion DllImport

        public float a = 0.001f;

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            DestroyPerceptron();
        }

        private void Init()
        {
            Entity[] entities = ObjectManager.Instance.GetComponentsInChildren<Entity>();

            int n = entities.Length;
            float[] input = new float[n * 2];
            float[] output = new float[n];

            Vector2 pos;
            for (int i = 0; i < n; i++)
            {
                pos = entities[i].transform.position;
                input[i * 2] = pos.x;
                input[i * 2 + 1] = pos.y;
                output[i] = entities[i].State - 1;
            }
            
            CreatePerceptron(a, n, 2);

            FillData(input, output);

            BackgroundManager.Instance.GetOutputFunc = GetOutput;
            PaintBackground();
        }

        public void RunPerceptron(int iterations)
        {
            int errors = Run(iterations);
            LogManager.Log("Running " + iterations + " iterations. Errors = " + errors);
            PaintBackground();
        }

        private void PaintBackground()
        {
            BackgroundManager.Instance.PaintYourself();
        }
    }
}