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

        public void PaintRegression(float wx, float wy, float b)
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
                    current.Value = sum;

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

        public void PaintMulticlass(float[,] weights)
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

                    float[] outputs = new float[weights.GetLength(1)];
                    for (int d = 0; d < outputs.Length; d++)
                    {
                        sum = pos.x * weights[0, d] + pos.y * weights[1, d] + weights[2, d];
                        outputs[d] = (float)Math.Tanh(sum);
                    }
                    
                    current.State = 0;
                    float max = -1000;
                    int maxID = 0;
                    for (int d = 0; d < outputs.Length; d++)
                    {
                        if (outputs[d] > max)
                        {
                            max = outputs[d];
                            maxID = d + 1;
                        }
                    }
                    
                    current.State = maxID;

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
    }
}