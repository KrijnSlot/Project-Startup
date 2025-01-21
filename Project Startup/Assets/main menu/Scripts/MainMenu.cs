using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // Add me!!


public class MainMenu : MonoBehaviour
{

    private Vector2 scale;
    private float minPitch = 0.9f;
    private float maxPitch = 1;

    public AudioSource ButtonAudio;

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPointerEnter()
    {
        // Debug.Log("The cursor entered the selectable UI element.");
        scale = transform.localScale;
        transform.localScale = scale + new Vector2(0.5f, 0.5f);
        ButtonAudio.pitch = Random.Range(minPitch, maxPitch);
        ButtonAudio.Play();
    }

    public void OnPointerExit()
    {
        // Debug.Log("The cursor entered the selectable UI element.");
        scale = transform.localScale;
        transform.localScale = scale + new Vector2(-0.5f, -0.5f);
        ButtonAudio.Stop();

    }
}
