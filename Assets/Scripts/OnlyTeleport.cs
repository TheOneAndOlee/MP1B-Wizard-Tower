using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlyTeleport : MonoBehaviour
{
    public string sceneName;
    public void Teleport()
    {
        SceneManager.LoadScene(sceneName);
    }
}
