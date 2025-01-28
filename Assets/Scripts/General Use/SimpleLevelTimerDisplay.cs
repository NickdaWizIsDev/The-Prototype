using TMPro;
using UnityEngine;

public class SimpleLevelTimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public bool alwaysDisplay;

    private void Update()
    {
        if(alwaysDisplay && displayText != null)
        {
            int min = Mathf.FloorToInt(Time.time / 60);
            int sec = Mathf.FloorToInt(Time.time % 60);
            displayText.text = min.ToString("00") + ":" + sec.ToString("00");
        }
    }

    public void SaveTime()
    {
        int min = Mathf.FloorToInt(Time.time / 60);
        int sec = Mathf.FloorToInt(Time.time % 60);
        displayText.text = min.ToString("00") + ":" + sec.ToString("00");
    }
}