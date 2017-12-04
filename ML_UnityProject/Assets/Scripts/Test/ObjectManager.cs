using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test
{
    public class ObjectManager : MonoBehaviour
    {
        public static ObjectManager Instance;

        public List<Entity> entities;

        void Awake()
        {
            ObjectManager.Instance = this;

            entities = new List<Entity>();
            foreach (var item in GetComponentsInChildren<Entity>())
            {
                entities.Add(item);
            }
        }
    }
}