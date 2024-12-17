using UnityEngine;
using UnityEngine.SceneManagement;  // For scene loading

public class PlayerDeath : MonoBehaviour
{
    // Function triggered when the player enters the trigger area (the ground)
    private void OnCollisionEnter(Collision collision)
    {
        // You can check the name of the tag for the ground if needed
        if (collision.gameObject.CompareTag("GroundDeath"))
        {
            // Call the function to handle death and restart
            RestartLevel();
        }
    }

    // Function to restart the level
    private void RestartLevel()
    {
        // Get the current scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}