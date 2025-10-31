using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.25f; 
    public float floatFrequency = 2f;    

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; 

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Coin collected!");

            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin(1);
            }

            Destroy(gameObject);
        }
    }
}
