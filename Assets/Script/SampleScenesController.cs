using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleScenesController : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
