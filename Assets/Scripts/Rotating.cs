using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.25f; // How high/low it floats
    public float floatFrequency = 2f;    // How fast it floats

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; // Degrees per second

    private Vector3 startPos;

    void Start()
    {
        // Save the starting position so we can float relative to it
        startPos = transform.position;
    }

    void Update()
    {
        // Floating effect (sine wave up/down)
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Rotate around Y axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player")) // Vehicle has the "Player" tag
    {
        Debug.Log("Coin collected!");
        Destroy(gameObject); // removes the coin
    }
}

}
