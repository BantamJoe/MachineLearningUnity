using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MachineLearning.Scene
{
    public class LogManager : MonoBehaviour
    {
        public static LogManager Instance;

        private static Text text;

        void Awake()
        {
            Instance = this;
            text = GetComponent<Text>();
        }

        public static void Log(string s)
        {
            Debug.Log(s);
            text.text = s;
        }
    }
}