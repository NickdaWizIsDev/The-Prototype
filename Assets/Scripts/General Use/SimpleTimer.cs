using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.General_Use
{
    public class SimpleTimer : MonoBehaviour
    {
        public UnityEvent OnComplete;
        float timer;
        bool timerStart;

        // Update is called once per frame
        void Update()
        {
            if(timerStart)
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    OnComplete.Invoke();
                    timer = 0;
                    timerStart = false;
                }
            }
        }

        public void TimerStart(float time)
        {
            timer = time;
            timerStart = true;
        }
    }
}