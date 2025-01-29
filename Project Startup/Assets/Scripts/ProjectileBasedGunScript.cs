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

    public PickUpScript mainCamPickUpScript;

    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    private bool canShoot;

    InputEvents inputEvents => InputEvents.instance;

    void Start()
    {
        canShoot = true;

        inputEvents.triggerAction += FireWeapon;
    }

    private void FireWeapon()
    {
        if(mainCamPickUpScript.holdingGun)
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
