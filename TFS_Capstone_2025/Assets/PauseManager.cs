using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI

    private bool isPaused = false; // Tracks if the game is paused

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Resume the game if it is currently paused
            }
            else
            {
                PauseGame(); // Pause the game if it is currently running
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); // Activate the pause menu
        Time.timeScale = 0f; // Freeze the game by setting time scale to 0
        isPaused = true; // Set the paused state to true
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false); // Deactivate the pause menu
        Time.timeScale = 1f; // Resume the game by resetting time scale to 1
        isPaused = false; // Set the paused state to false
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game..."); // Log a message (for debugging purposes)
        Application.Quit(); // Exit the application
    }
}
