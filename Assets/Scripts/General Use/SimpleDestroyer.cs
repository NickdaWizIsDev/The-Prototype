using System.Collections;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public class SimpleDestroyer : MonoBehaviour
    {
        public GameObject obj;

        bool count;

        public void DestroyGameObject(GameObject go)
        {
            Destroy(go);
        }

        public void DestroyThis()
        {
            Destroy(gameObject);
        }

        public void DestroyComponent(Component component)
        {
            Destroy(component);
        }

        public void DestroyAfterTime(float time)
        {
            Destroy(gameObject, time);
        }
    }
}