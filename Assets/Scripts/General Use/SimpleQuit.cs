using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public class SimpleQuit : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
#if(UNITY_EDITOR)
            EditorApplication.isPlaying = false;
#endif
        }
    }
}