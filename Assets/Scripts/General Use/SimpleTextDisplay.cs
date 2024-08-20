using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public class SimpleTextDisplay : MonoBehaviour
    {
        public TextMeshProUGUI textMeshProUGUI;

        SimpleTimer timer;

        private void Awake()
        {
            timer = GetComponent<SimpleTimer>();
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public void DisplayText(string text)
        {
            textMeshProUGUI.text = text;
        }

        public void HideText(float time)
        {
            timer.TimerStart(time);
        }
    }
}