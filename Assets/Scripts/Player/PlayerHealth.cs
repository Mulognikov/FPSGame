using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public static PlayerHealth PH;

    public delegate void PlayerHealthChangedEventHandler(int health);
    public delegate void PlayerDeadEventHandler();

    public static event PlayerHealthChangedEventHandler PlayerHealthChangedEvent;
    public static event PlayerDeadEventHandler PlayerDeadEvent;

    [SerializeField] private int health;
    [SerializeField] private BleedBehavior bleedBehavior;

    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioSource audioSource;

    private int normalHealth;

    private void Awake()
    {
        PH = this;
        normalHealth = health;
    }

    private void Start()
    {
        PlayerHealthChangedEvent?.Invoke(health);
    }

    public void GetDamage(int damage)
    {
        if (health <= 0 || Time.timeScale != 1)
            return;

        if (UnityEngine.Random.Range(0, 1f) < 0.4f)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);
        }

        health -= damage;

        if (health <= 0)
        {
            health = 0;

            audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);

            if (PlayerTimer.PT.GetTimeLeft() > 0)
                StartCoroutine(Routine());

            IEnumerator Routine()
            {
                Time.timeScale = 0.999f;
                ScreenFaderProvider.FadeIn(1f);

                yield return new WaitForSeconds(1f);

                PlayerDeadEvent?.Invoke();
                if (PlayerTimer.PT.GetTimeLeft() > 0)
                    RespawnPlayer();

                yield return new WaitForSeconds(1f);

                if (PlayerTimer.PT.GetTimeLeft() > 0)
                    ScreenFaderProvider.FadeOut(1f);

                Time.timeScale = 1f;
            }
        }

        BleedBehavior.BloodAmount += ( Mathf.Log(Mathf.Pow(damage, 15) ) / normalHealth) + 0.35f;
        //BleedBehavior.BloodAmount += damage / normalHealth * 5 + 0.35f;

        PlayerHealthChangedEvent?.Invoke(health);
    }

    private void RespawnPlayer()
    {
        transform.position = PlayerRespawnPoint.RespawnPoint.position;
        transform.rotation = PlayerRespawnPoint.RespawnPoint.rotation;
        health = normalHealth;
        PlayerHealthChangedEvent?.Invoke(health);
    }

    public int GetHealth()
    {
        return health;
    }

    public void RestoreHealth(int ResHealth)
    {
        health += ResHealth;

        if (health > 100)
        {
            health = 100;
        }

        PlayerHealthChangedEvent?.Invoke(health);
    }
}
