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
        public static extern float GetOutput(float[] input, bool useBest);
        #endregion DllImport

        public float a = 0.001f;
        bool isCreated = false;

        private void Start()
        {
            Init();
            isCreated = true;
        }
        
        private void OnDestroy()
        {
            if (isCreated)
            {
                DestroyPerceptron();
            }
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
                input[i] = pos.x;
                input[i + n] = pos.y;
                output[i] = entities[i].State == 2 ? 1 : -1;
            }
            
            CreatePerceptron(a, n, 2);

            FillData(input, output);

            BackgroundManager.Instance.GetOutputFunc = GetOutput;
            PaintBackground();
        }

        public int RunPerceptron(int iterations)
        {
            int errors = Run(iterations);
            LogManager.Log("Running " + iterations + " iterations. Errors = " + errors);
            PaintBackground();
            return errors;
        }

        private void PaintBackground()
        {
            BackgroundManager.Instance.PaintYourself();
        }
    }
}