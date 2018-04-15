using UnityEngine;

namespace MachineLearning.Scene
{
    public class Entity : MonoBehaviour
    {
        private Material mat;
        
        private int state = 0;
        public int State {
            get { return state; }
            set {
                if (state < Consts.colors.Length)
                {
                    state = value;
                    mat.color = Consts.colors[state];
                }
                else
                {
                    Debug.LogWarning("[Entity] No color for state " + value);
                }
            }
        }

        private float val = 0;
        public float Value {
            get { return val; }
            set {
                val = Mathf.Clamp01(value);
                mat.color = new Color(val, 0, 1 - val);
            }
        }

        private void Awake()
        {
            mat = GetComponent<MeshRenderer>().material;
            for (int i = 0; i < Consts.colors.Length; i++)
            {
                if (mat.color == Consts.colors[i])
                {
                    state = i;
                    break;
                }
            }
            val = mat.color.r;
        }
    }
}