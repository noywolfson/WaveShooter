using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    // Variables for the Gun
    [SerializeField] new Camera camera; // Reference the camera, needed for raycasting
    [SerializeField] int damage = 1; // Set the default damage to enemy
    [SerializeField] int range = 100; //Range of raycast
    [SerializeField] int clipSize = 7;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] int currentAmmo;
    bool isReloading = false;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Animator animator;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();

        if(currentAmmo <= 0)
        {
            currentAmmo = clipSize;
        }

        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if reloading
        if(isReloading)
        {
            return;
        }

        //Check our current ammo
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        // Check Input
        if(Input.GetButtonDown("Fire1"))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        currentAmmo--;

        muzzleFlash.Play();

        RaycastHit hit;
        //This will check if the raycast hits anything
        if (Input.GetMouseButton(0))
        { // your code here 

            audioManager.Play("Shoot");

            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
            {
                // Check if raycast works, send message to console
                print("You hit " + hit.transform.name);

                //Reference out Enemy Health script
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();

                //check if shot enemy
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamange(damage);
                    audioManager.Play("ZombiePain");
                }
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        print("Reloading");

        animator.SetBool("isReloading", true);

        audioManager.Play("Reload");
        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("isReloading", false);

        currentAmmo = clipSize;
        isReloading = false;
    }
}
