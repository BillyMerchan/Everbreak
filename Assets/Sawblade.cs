using UnityEngine;

public class Sawblade : MonoBehaviour
{
    public float speed = 3f; // Speed of the sawblade movement
    public float distance = 5f; // Total distance for back-and-forth movement along the z-axis
    private Vector3 startPosition; // Initial position of the sawblade

    private bool movingForward = true; // Direction of movement

    public float rotationSpeed = 100f; // Rotation speed of the sawblade
    public Renderer bladeRenderer; // Renderer of the sawblade to change color
    public Color glowColor = Color.red; // Color of the glowing effect
    public AudioClip warningSoundClip; // The warning sound clip to play when called
    public float warningDistance = 5f; // Distance at which the warning is triggered

    private bool isPlayerNear = false; // Flag to check if player is near
    private bool isSinging = false; // Flag to check if the player is singing

    void Start()
    {
        // Record the starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the sawblade along the z-axis
        float move = speed * Time.deltaTime;
        if (movingForward)
        {
            transform.position += new Vector3(0, 0, move);

            // Reverse direction if it moves too far forward
            if (transform.position.z >= startPosition.z + distance)
            {
                movingForward = false;
            }
        }
        else
        {
            transform.position -= new Vector3(0, 0, move);

            // Reverse direction if it moves too far back
            if (transform.position.z <= startPosition.z - distance)
            {
                movingForward = true;
            }
        }

        // Rotate the sawblade around the Z-axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Check if player is near the blade
        CheckPlayerProximity();

        // Listen for 'E' key press for the singing interaction
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNear)
        {
            isSinging = true;
            TriggerWarning();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isSinging = false;
            StopWarning();
        }
    }

    // Check if the player is within the warning distance
    private void CheckPlayerProximity()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= warningDistance)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                StopWarning();
            }
        }
    }

    // This method explicitly triggers the warning and sound
    private void TriggerWarning()
    {
        // Glow the blade if the player is singing near the blade
        if (bladeRenderer != null)
        {
            bladeRenderer.material.SetColor("_EmissionColor", glowColor); // Glow effect
        }

        // Play the warning sound if the player is near and singing
        if (warningSoundClip != null && isSinging)
        {
            AudioSource.PlayClipAtPoint(warningSoundClip, transform.position);
        }
    }

    // Stop the glowing effect
    private void StopWarning()
    {
        if (bladeRenderer != null)
        {
            bladeRenderer.material.SetColor("_EmissionColor", Color.black); // Stop glowing
        }
    }
}