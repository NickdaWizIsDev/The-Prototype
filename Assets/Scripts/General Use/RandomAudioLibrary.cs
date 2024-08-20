using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Library")]
public class RandomAudioLibrary : ScriptableObject
{
    public AudioClip[] myAudioFiles;

    public AudioClip Clip
    {
        // devuelve un sonido aleatorio de la libreria
        get { return myAudioFiles[Random.Range(0, myAudioFiles.Length)]; }
    }

}