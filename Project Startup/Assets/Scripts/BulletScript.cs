using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public AudioClip hitSound;

    [SerializeField]
    private string requiredTag;
    [SerializeField]
    private string fish;
    [SerializeField]
    private string shark;


    [HideInInspector] public static Bullet instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag(requiredTag))
        {
            if (CompareTag(fish))
            {

            }
            if (CompareTag(shark))
            {

            }
            /*if (hitSound != null) // makes a seperate gameobject, containing the audio source, to play the sound, without using delay in destroying the object.
            {
                GameObject audioObject = new GameObject("sfx", typeof(AudioSource));
                AudioSource src = audioObject.GetComponent<AudioSource>();
                audioObject.transform.position = gameObject.transform.position;
                src.clip = hitSound;
                src.spatialBlend = 0.75f;
                src.volume = 40;
                src.Play();
                Destroy(audioObject, hitSound.length);
            }*/
            /*print("hit " + collision.gameObject.name + " !");*/
            Destroy(gameObject);
        }
    }
}
