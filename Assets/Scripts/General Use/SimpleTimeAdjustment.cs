using System.Collections;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public class SimpleTimeAdjustment : MonoBehaviour
    {
        public float speedyTime;
        public float slowTime;

        // Use this for initialization
        void Start()
        {
            Time.timeScale = 1;
        }

        public void SpeedUp()
        {
            Time.timeScale = speedyTime;
        }

        public void SpeedDown()
        {
            Time.timeScale = slowTime;
        }

        public void SpeedZero()
        {
            Time.timeScale = 0;
        }

        public void SpeedOne()
        {
            Time.timeScale = 1;
        }
    }
}