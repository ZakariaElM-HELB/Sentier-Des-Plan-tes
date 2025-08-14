using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScenes");
    }
}
