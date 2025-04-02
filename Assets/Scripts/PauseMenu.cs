using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public static bool isPause = false;

    public GameObject pauseMenuUi;
    public GameObject[] objectsToHide; // Vetor de objetos a serem ocultados/ativados

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void voltarMenu(string sceneName){
        Resume();
        SceneManager.LoadScene(sceneName);
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;

        // Ativa os objetos novamente, se houver objetos no vetor
        if (objectsToHide.Length > 0)
        {
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(true);
            }
        }
    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;

        // Desativa os objetos, se houver objetos no vetor
        if (objectsToHide.Length > 0)
        {
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(false);
            }
        }
    }

    public static void PauseAnd(GameObject[] objectsToUnhide, GameObject[] hide, int mode)
    {
        if (Instance == null) return;

        Instance.pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;

        if (Instance.objectsToHide.Length > 0)
        {
            foreach (GameObject obj in Instance.objectsToHide)
            {
                obj.SetActive(false);
            }
        }

        if (mode == 1)
        {
            foreach (GameObject obj in objectsToUnhide)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in hide)
            {
                obj.SetActive(false);
            }
        }
    }

}
