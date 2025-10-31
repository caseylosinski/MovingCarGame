using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Explosion Effect Prefab")]
    public GameObject explosionEffect;

    [Header("Sound Effect (optional)")]
    public AudioClip hitSound;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Player hit an obstacle!");

            // Spawn explosion at obstacle position
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // Optional: play sound effect at impact
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            // Tell CoinManager to remove one coin
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.RemoveCoin(1);
            }

            // Destroy the obstacle so it doesn't trigger again
            Destroy(gameObject);
        }
    }
}
