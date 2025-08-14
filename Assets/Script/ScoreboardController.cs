using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardController : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
