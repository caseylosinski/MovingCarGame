using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("Scene to load when player wins")]
    public string mainMenuSceneName = "Main Menu"; // or whatever your menu scene is named

    [Header("Optional delay before returning (seconds)")]
    public float delayBeforeReturn = 2f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return; // prevent double triggers

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Player reached the goal! You win!");

            // Optionally show a message on screen here (UI popup)

            // Return to menu after a short delay
            Invoke(nameof(ReturnToMenu), delayBeforeReturn);
        }
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
