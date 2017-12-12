using System;
using UnityEngine;

namespace MachineLearning.Scene
{
    public class BackgroundManager : MonoBehaviour
    {
        public static BackgroundManager Instance;

        public delegate float GetOutPutDelegate(float[] input, bool useBest);
        public GetOutPutDelegate GetOutputFunc;

        public GameObject prefab;
        public int sizeX;
        public int sizeY;
        public Vector3 offset;

        [Header("Press 'A' to use debug weights.")]
        public float[] debugWeights = { 1, 1, 0 };

        private Entity[] bgElements;

        void Awake()
        {
            BackgroundManager.Instance = this;

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

            Debug.Log("BackgroundManager is awoken.");
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

        public void Paint2Layer(float[] weights)
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

                    sum = pos.x * weights[0] + pos.y * weights[1] + weights[2];
                    float a = (float)Math.Tanh(sum);
                    sum = pos.x * weights[3] + pos.y * weights[4] + weights[5];
                    float b = (float)Math.Tanh(sum);

                    sum = a * weights[6] + b * weights[7] + weights[8];
                    int result = Math.Sign(sum);
                    
                    current.State = result >= 0 ? 1 : 2;

                    ++pos.x;
                }
                ++pos.y;
            }
        }

        public void PaintMultiClasses(float[,] weights)
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

                    sum = pos.x * weights[0, 0] + pos.y * weights[1, 0] + weights[2, 0];
                    float y1 = (float)Math.Tanh(sum);
                    sum = pos.x * weights[0, 1] + pos.y * weights[1, 1] + weights[2, 1];
                    float y2 = (float)Math.Tanh(sum);
                    sum = pos.x * weights[0, 2] + pos.y * weights[1, 2] + weights[2, 2];
                    float y3 = (float)Math.Tanh(sum);
                    
                    // Old method : everything not full-one-colored is white. 
                    //current.State = 0;
                    //if (y1 > 0 && y2 < 0 && y3 < 0)
                    //    current.State = 1;
                    //else if (y1 < 0 && y2 > 0 && y3 < 0)
                    //    current.State = 2;
                    //else if (y1 < 0 && y2 < 0 && y3 > 0)
                    //    current.State = 3;

                    current.State = 0;
                    if (y1 > y2 && y1 > y3)
                        current.State = 1;
                    else if (y2 > y1 && y2 > y3)
                        current.State = 2;
                    else if (y3 > y1 && y3 > y2)
                        current.State = 3;

                    ++pos.x;
                }
                ++pos.y;
            }
        }

        public void PaintYourself()
        {
            Vector2 startPos = new Vector3(-sizeX / 2, -sizeY / 2);
            Vector2 pos = startPos + (Vector2)offset;

            Entity current;
            float output;

            float[] p = new float[2];

            for (int y = 0; y < sizeY; ++y)
            {
                pos.x = startPos.x + offset.x;
                p[1] = pos.y;
                for (int x = 0; x < sizeX; ++x)
                {
                    p[0] = pos.x;
                    current = bgElements[y * (sizeX) + x];
                    output = GetOutputFunc(p, true);
                    current.State = output == 0 ? 0 : output > 0 ? 2 : 1;

                    ++pos.x;
                }
                ++pos.y;
            }
        }

        public void Paint2Layer(float[,,] weights)
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

                    sum = pos.x * weights[0, 0, 0] + pos.y * weights[0, 0, 1] + weights[0, 0, 2];
                    float a = (float)Math.Tanh(sum);
                    sum = pos.x * weights[0, 1, 0] + pos.y * weights[0, 1, 1] + weights[0, 1, 2];
                    float b = (float)Math.Tanh(sum);
                    sum = pos.x * weights[0, 2, 0] + pos.y * weights[0, 2, 1] + weights[0, 2, 2];
                    float c = (float)Math.Tanh(sum);

                    sum = a * weights[1, 0, 0] + b * weights[1, 1, 0] + c * weights[1, 2, 0] + weights[1, 0, 1];
                    sum = (float)Math.Tanh(sum);
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