using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    void Update()
    {
        // Verifica se a tecla P foi pressionada para pausar/despausar o jogo
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pausa o jogo
        }
        else
        {
            Time.timeScale = 1f; // Retoma o jogo
        }
    }
}