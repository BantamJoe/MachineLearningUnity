using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test
{
    public class ObjectCreation : MonoBehaviour
    {
        public static ObjectCreation Instance;

        public GameObject prefab;
        public Vector2 area = Vector2.one;
        public int quantity = 10;
        public Color c1 = Color.blue;
        public Color c2 = Color.red;

        void Awake()
        {
            float a = Random.Range(-10, 10);
            float b = Random.Range(-10, 10);

            Instance = this;

            Vector3 rd = Vector3.zero;
            Entity ent;

            for (int i = 0; i < quantity; i++)
            {
                rd.x = Random.Range(-area.x, area.x);
                rd.y = Random.Range(-area.y, area.y);
                ent = Instantiate(prefab, rd, Quaternion.identity, transform).GetComponent<Entity>();
                
                ent.State = ent.transform.position.y > a * ent.transform.position.x + b ? 1 : 0;
            }
        }
    }
}