using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int hitsToTake = 3;
    [SerializeField] float respawnTime = 3f;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject hitScreen;
    [SerializeField] GameObject gun;

    LevelController levelController;

    AudioManager audioManager;

    private void Start()
    {
        deathScreen.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        levelController = FindObjectOfType<LevelController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            if (hitsToTake > 0)
            {
                print("Enemy touched the player");
                StartCoroutine(PlayerHit());
                hitsToTake--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            if(hitsToTake <= 0)
            {
                print("Player has died");
                StartCoroutine(PlayerDeath());
                return;
            }
    }

    IEnumerator PlayerDeath()
    {
        deathScreen.SetActive(true);
        audioManager.Play("PlayerDie");
        gun.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        levelController.GameOver();
    }
    
    IEnumerator PlayerHit()
    {
        hitScreen.SetActive(true);
        audioManager.Play("Hit");
        yield return new WaitForSeconds(0.1f);
        hitScreen.SetActive(false);
    }
}
