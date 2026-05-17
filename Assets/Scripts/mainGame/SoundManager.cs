using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Main Game")]
    public AudioClip keyPickupSound;
    public AudioClip gunShotSound;

    [Header("Zelda")]
    public AudioClip swordPickupSound;
    public AudioClip swordSwingSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayKeyPickup()
    {
        PlaySound(keyPickupSound);
    }

    public void PlayGunShot()
    {
        PlaySound(gunShotSound);
    }

    public void PlaySwordPickup()
    {
        PlaySound(swordPickupSound);
    }

    public void PlaySwordSwing()
    {
        PlaySound(swordSwingSound);
    }
}