using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    // Detect when the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged "Player"
        if (other.CompareTag("Player"))
        {
            // Print a message for debugging
            Debug.Log("Player has reached the trigger!");

            // Load the Main Menu scene (change the scene name accordingly)
            SceneManager.LoadScene("MainMenu");
        }
    }
}