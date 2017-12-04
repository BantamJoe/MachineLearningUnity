using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test
{
    public class BackgroundCreation : MonoBehaviour
    {
        public GameObject prefab;
        public Vector2 area = Vector2.one;

        void Awake()
        {
            Vector3 pos = transform.position;
            for (pos.y = -area.y; pos.y <= area.y; ++pos.y)
            {
                for (pos.x = -area.x; pos.x <= area.x; ++pos.x)
                {Instantiate(prefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }
}