using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;  // player's start lives
    public Text livesText;  // UI Text to display lives (implement later maybe?)
    public GameObject gameOverPanel;  // Game Over UI panel to display when lives are zero (implement later maybe?)

    void Start()
    {
        // display the starting number of lives
        // UpdateLivesText();
        
        //  Game Over panel is hidden at the start
        // if (gameOverPanel != null)
        // {
        //    gameOverPanel.SetActive(false);
        // }
    }

    // player is hit by bot
    public void TakeDamage1()
    {
        if (lives > 0)
        {
            lives--;  // decrease player's lives by 1
            // UpdateLivesText();  // update UI to show the new number of lives

            // check if the player has no lives left
            if (lives <= 0)
            {
                GameOver();
            }
        }
    }

    // update UI text showing the player's remaining lives
    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives.ToString();
        }
    }

    // Handles the game over condition
    private void GameOver()
    {
        Debug.LogError("Game Over!");
        
        GameLoop gameLoop = FindObjectOfType<GameLoop>();
        if (gameLoop != null)
        {
            gameLoop.TriggerGameOver();
        }
        else
        {
            Debug.LogError("GameLoop not found in scene!");
        }

        Invoke("PauseGame", .2f); // call PauseGame after 2 seconds
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

}
