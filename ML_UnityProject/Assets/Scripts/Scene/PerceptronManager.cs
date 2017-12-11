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

        void Awake()
        {
            p = GetComponent<Perceptron>();
            dlp = GetComponent<DualLayerPerceptron>();
            sp = GetComponent<SimplePerceptron>();
        }

        public void ChangePerceptron(Dropdown dropDown)
        {
            p.enabled = dropDown.value == 0 ? true : false;
            dlp.enabled = dropDown.value == 1 ? true : false;
            sp.enabled = dropDown.value == 2 ? true : false;
        }

        public void RunPerceptron(int iteration)
        {
            if (p.enabled)
            {
                p.RunPerceptron(iteration);
            }
            else if (dlp.enabled)
            {
                dlp.RunPerceptron(iteration);
            }
            else if (sp.enabled)
            {
                sp.RunPerceptron(iteration);
            }
        }
    }
}