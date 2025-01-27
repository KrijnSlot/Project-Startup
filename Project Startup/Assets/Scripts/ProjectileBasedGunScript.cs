using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBasedGunScript : MonoBehaviour
{
    [Header("Sounds")]
    public AudioSource Gunshot;

    [Header("Bullet Info")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    private bool canShoot;

    InputEvents inputEvents => InputEvents.instance;


    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;

        inputEvents.triggerAction += FireWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canShoot);
    }

    private void FireWeapon()
    {
        if (!canShoot)
            return;
        Gunshot.Play();

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        bullet.transform.rotation = bulletSpawn.rotation;

        Destroy(bullet, bulletPrefabLifeTime);
        canShoot = false;
        StartCoroutine(fireRate(0.25f));
    }

    private IEnumerator fireRate(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
