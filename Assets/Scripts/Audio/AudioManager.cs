using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip placeSound;
    public AudioClip hitSound;
    public AudioClip missSound;
    public AudioClip sunkSound;
    public AudioClip victorySound;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void PlayPlace() {
        audioSource.PlayOneShot(placeSound);
    }

    public void PlayHit() {
        audioSource.PlayOneShot(hitSound);
    }

    public void PlayMiss() {
        audioSource.PlayOneShot(missSound);
    }

    public void PlaySunk() {
        audioSource.PlayOneShot(sunkSound);
    }

    public void PlayVictory() {
        audioSource.PlayOneShot(victorySound);
    }
}