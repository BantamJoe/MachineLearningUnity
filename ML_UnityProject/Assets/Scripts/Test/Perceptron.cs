using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test
{
    public class Perceptron : MonoBehaviour
    {
        public float a = 0.05f;

        public BackgroundCreation background;
        public ObjectManager objects;

        private List<Transform> bg = new List<Transform>();

        public List<float> factors;

        private void Start()
        {
            foreach (Transform child in background.transform)
            {
                bg.Add(child);
            }

            factors = new List<float>();
            for (int i = 0; i < 3; i++)
            {
                factors.Add(Random.Range(-1f, 1f));
            }

            PaintBackground();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
            {
                GetBetter();
            }
        }

        public void GetBetter()
        {
            List<Entity> tmp = new List<Entity>(objects.entities);
            


            for (int i = tmp.Count - 1; i >= 0; i--)
            {
                Transform t = tmp[Random.Range(0, i + 1)].transform;
                float val = t.position.x * factors[0] +
                    t.position.y * factors[1] +
                    factors[2];
                val = val > 0 ? 1 : 0;

                if (tmp[i].State == val)
                {
                    tmp.Remove(tmp[i]);
                    continue;
                }
                else
                {
                    factors[0] += a * (t.GetComponent<Entity>().State - val) * t.position.x;
                    factors[1] += a * (t.GetComponent<Entity>().State - val) * t.position.y;
                    factors[2] += a * (t.GetComponent<Entity>().State - val);
                    break;
                }
            }

            PaintBackground();
        }

        private void PaintBackground()
        {
            foreach (Entity child in background.transform.GetComponentsInChildren<Entity>())
            {
                float val = child.transform.position.x * factors[0] +
                    child.transform.position.y * factors[1] +
                    factors[2];

                child.State = val > 0 ? 1 : 0;
            }
        }
    }
}