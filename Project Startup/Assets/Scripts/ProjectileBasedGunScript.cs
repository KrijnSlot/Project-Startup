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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        Gunshot.Play();

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        bullet.transform.rotation = bulletSpawn.rotation;

        Destroy(bullet, bulletPrefabLifeTime);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
