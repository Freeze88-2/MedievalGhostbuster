using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private MenuButtonController _menuButtonController;
    public bool disableOnce;

    private void Playsound(AudioClip sound)
    {
        if(!disableOnce)
            _menuButtonController.audioSource.PlayOneShot(sound);
        else
            disableOnce = false;
    }
}
