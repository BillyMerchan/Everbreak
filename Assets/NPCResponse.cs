using UnityEngine;

public class NPCSoundTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private bool playerInRange = false; // Track if player is near the NPC

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void PlayNPCSound()
    {
        // Play the sound only if the player is in range and the sound is not already playing
        if (playerInRange && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}