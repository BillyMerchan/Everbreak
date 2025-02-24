using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioClip[] footstepClips; // Array to store your footstep sounds
    private AudioSource audioSource;
    public float stepInterval = 0.5f; // Time between footsteps
    private float stepTimer;

    private CharacterController characterController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the character is moving
        if (characterController.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepClips.Length > 0)
        {
            // Pick a random clip
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}