using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 1;
    public AudioClip hitSound;

    SharkScript sharkScript;

    private void Start()
    {
        //sharkScript = GetComponent<SharkScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("isShark"))
        {
            SharkScript shark = collision.transform.GetComponent<SharkScript>();
            if (shark != null)
            {
                shark.SharkHit();
            }
        }
        Destroy(gameObject);
    }

}
