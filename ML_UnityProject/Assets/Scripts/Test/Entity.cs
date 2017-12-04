using UnityEngine;

namespace test
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

        private void Start()
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
        }

        private void SetColor(Color c)
        {
            mat.color = c;
        }
    }
}