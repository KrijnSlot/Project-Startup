using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GunShootingScript : MonoBehaviour
{
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, fireRate;
    public int magSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public PickUpScript pickupScript;

    public Camera cam;
    public Transform attackPoint;
    public AudioSource src;
    public AudioClip shootSound, reloadSound;
    public RaycastHit hit;
    public LayerMask isEnemy;

    public GameObject muzzleFlash, bulletHoleGraphic, bulletHitEnemy;

    private void Start()
    {
        bulletsLeft = magSize;
        readyToShoot = true;

        /*ScoreScript.bulletCount = bulletsLeft;*/
        /*ScoreScript.magSizeCount = magSize;*/
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if ((Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading) || bulletsLeft <= 0 && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {

        src.PlayOneShot(shootSound, 0.5f);

        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(cam.transform.position, direction, out hit, range, isEnemy))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                /*hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);*/
                GameObject obj = Instantiate(bulletHitEnemy, hit.point, Quaternion.LookRotation(hit.normal));
                obj.transform.parent = hit.transform;
            }
            else if (hit.collider.CompareTag("Teleport"))
            {
                SceneManager.LoadScene("WaveScene");
            }
            else
            {
                Instantiate(bulletHoleGraphic, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        Instantiate(muzzleFlash, attackPoint);

        bulletsLeft--;
        bulletsShot--;

        /*ScoreScript.bulletCount = bulletsLeft;*/

        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", fireRate);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        src.PlayOneShot(reloadSound);

        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;

        /*ScoreScript.bulletCount = bulletsLeft;*/
    }
}