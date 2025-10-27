using UnityEngine;

public class CoinSound : MonoBehaviour
{
    // Drag your AudioSource here in the Inspector
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the Vehicle
        if (other.CompareTag("Coin"))
        {
            // Play the coin sound if assigned
            if (audioSource != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource not assigned on " + gameObject.name);
            }
        }
    }
}
