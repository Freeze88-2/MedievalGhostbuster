using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseFunctions : MonoBehaviour
{
    [SerializeField] private AudioMixer     _mixer;
    [SerializeField] private AudioListener  _listener;

    private bool mute = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GoLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void VolumeSlider(float volumeValue)
    {
        _mixer.SetFloat("SceneMasterExp", Mathf.Log10(volumeValue) * 20);
    }

    public void Mute()
    {
        mute = !mute;

        if (mute == true)
            _listener.enabled = false;
        
        else if (mute == false)
            _listener.enabled = true;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitToDesk()
    {
        Application.Quit();
    }
}
