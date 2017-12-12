using UnityEngine;
using UnityEngine.UI;
using MachineLearning.Algo;

namespace MachineLearning.Scene
{
    public class PerceptronManager : MonoBehaviour
    {
        Perceptron p;
        DualLayerPerceptron dlp;
        SimplePerceptron sp;
        MulticlassPerceptron dop;

        Toggle runToggle;

        public bool running = false;

        void Awake()
        {
            GameObject go = GameObject.FindWithTag("Perceptron");

            if (go == null)
            {
                Debug.LogError("ERROR : No gameobject with Perceptron tag :(.");
                enabled = false;
                return;
            }

            p = go.GetComponent<Perceptron>();
            dlp = go.GetComponent<DualLayerPerceptron>();
            sp = go.GetComponent<SimplePerceptron>();
            dop = go.GetComponent<MulticlassPerceptron>();

            runToggle = GetComponentInChildren<Toggle>();
        }

        public void ChangePerceptron(int value)
        {
            p.enabled = value == 0 ? true : false;
            dlp.enabled = value == 1 ? true : false;
            sp.enabled = value == 2 ? true : false;
            dop.enabled = value == 3 ? true : false;
        }

        void Update()
        {
            if (running)
            {
                int errors = RunPerceptronAndGetError(10);
                if (errors <= 0)
                {
                    runToggle.isOn = false;
                }
            }
        }

        public void ToggleRunPerceptron(bool value)
        {
            running = value;
        }

        public void RunPerceptron(int iteration)
        {
            RunPerceptronAndGetError(iteration);
        }

        public int RunPerceptronAndGetError(int iteration)
        {
            if (p.enabled)
            {
                return p.RunPerceptron(iteration);
            }
            else if (dlp.enabled)
            {
                return dlp.RunPerceptron(iteration);
            }
            else if (sp.enabled)
            {
                return sp.RunPerceptron(iteration);
            }
            else if (dop.enabled)
            {
                return dop.RunPerceptron(iteration);
            }

            return 0;
        }
    }
}