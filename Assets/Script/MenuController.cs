using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LaunchMission()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void GalacticRankings()
    {
        SceneManager.LoadScene("ScoreboardScene");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
