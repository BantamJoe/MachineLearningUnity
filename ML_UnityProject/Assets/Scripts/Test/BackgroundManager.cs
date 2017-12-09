using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test
{
    public class BackgroundManager : MonoBehaviour
    {
        public GameObject prefab;
        public int sizeX;
        public int sizeY;
        public Vector3 offset;

        [Header("Press 'A' to use debug weights.")]
        public float[] debugWeights = { 1, 1, 0 };

        private Entity[] bgElements;

        void Awake()
        {
            Vector3 startPos = new Vector3(-sizeX / 2, -sizeY / 2);
            Vector3 pos = startPos + offset;
            Vector3 limit = pos + new Vector3(sizeX, sizeY);
            for (; pos.y < limit.y; ++pos.y)
            {
                for (pos.x = startPos.x + offset.x; pos.x < limit.x; ++pos.x)
                {
                    Instantiate(prefab, pos, Quaternion.identity, transform);
                }
            }

            bgElements = GetComponentsInChildren<Entity>();
        }

        public void Paint(float wx, float wy, float b)
        {
            Vector2 startPos = new Vector3(-sizeX / 2, -sizeY / 2);
            Vector2 pos = startPos + (Vector2)offset;
            
            Entity current;
            float sum;

            for (int y = 0; y < sizeY; ++y)
            {
                pos.x = startPos.x + offset.x;
                for (int x = 0; x < sizeX; ++x)
                {
                    current = bgElements[y * (sizeX) + x];
                    sum = pos.x * wx + pos.y * wy + b;
                    current.State = sum == 0 ? 0 : sum > 0 ? 2 : 1;

                    ++pos.x;
                }
                ++pos.y;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Paint(debugWeights[0], debugWeights[1], debugWeights[2]);
            }
        }
    }
}