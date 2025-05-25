using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseEndScript : MonoBehaviour
{
    [SerializeField] GameObject panel;
    /*public void OnRestart()
    {
        
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        intro.SetActive(true);
    }*/

    public void MainMenu()
    {
        panel.SetActive(true);
        // Unpause time just in case
        Time.timeScale = 1f;

        Destroy(GameObject.Find("Maps")); 
        SceneManager.LoadScene("Title Screen");
    }
}

