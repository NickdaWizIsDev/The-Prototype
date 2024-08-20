using System.Collections;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public class DoorSounds : MonoBehaviour
    {
        public AudioClip doorOpen;
        public AudioClip doorClose;
        AudioSource doorSource;

        private void Start()
        {
            doorSource = GetComponent<AudioSource>();
        }

        public void Open()
        {
            doorSource.PlayOneShot(doorOpen);
        }

        public void Close()
        {
            doorSource.PlayOneShot(doorClose);
        }
    }
}