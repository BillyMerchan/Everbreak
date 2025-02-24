using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip hurtClip; // Assign the hurt sound in the Inspector

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurtSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(hurtClip);
        }
    }
}