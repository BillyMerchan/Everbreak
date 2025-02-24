using UnityEngine;

public class CharacterSpeech : MonoBehaviour
{
    private AudioSource playerAudioSource;

    // Player's voice clips
    public AudioClip[] playerSpeechClips; // Array for player speech clips

    private int currentClipIndex = 0; // Default to the first clip
    private bool isNearNPC = false; // Flag to check if player is near an NPC

    private AudioSource npcAudioSource; // Reference to the NPC's AudioSource

    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();

        // Ensure we have valid player speech clips
        if (playerSpeechClips.Length == 0)
        {
            Debug.LogError("Player speech clips are not assigned.");
        }
    }

    void Update()
    {
        // Change the selected voice clip
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentClipIndex = 0;
            Debug.Log("Player selected clip 1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentClipIndex = 1;
            Debug.Log("Player selected clip 2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentClipIndex = 2;
            Debug.Log("Player selected clip 3");
        }

        // Play the selected speech clip when pressing 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayPlayerSpeech();
        }
    }

    void PlayPlayerSpeech()
    {
        if (!playerAudioSource.isPlaying && playerSpeechClips.Length > currentClipIndex)
        {
            // Play the selected audio clip
            playerAudioSource.clip = playerSpeechClips[currentClipIndex];
            playerAudioSource.Play();

            // Trigger NPC response if near an NPC
            if (isNearNPC && npcAudioSource != null)
            {
                StartCoroutine(DelayedNpcSound());
            }
        }
    }

    System.Collections.IEnumerator DelayedNpcSound()
    {
        yield return new WaitForSeconds(1.4f); // Delay for 0.7 seconds

        if (npcAudioSource != null)
        {
            npcAudioSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isNearNPC = true;
            npcAudioSource = other.GetComponent<AudioSource>();

            if (npcAudioSource == null)
            {
                Debug.LogError("NPC is missing an AudioSource component.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isNearNPC = false;
            npcAudioSource = null;
        }
    }
}