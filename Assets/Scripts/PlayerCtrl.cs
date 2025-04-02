using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public float movSpeed = 1f;
    public GameObject[] islands;

    void Start()
    {
        // Se ProgessManager já tem um índice válido, usa ele
        if (ProgessManager.Instance.currentIsland < islands.Length)
        {
            transform.position = islands[ProgessManager.Instance.currentIsland].transform.position;
        }
        else
        {
            ProgessManager.Instance.currentIsland = 0;
            transform.position = islands[0].transform.position;
        }

        // Garantir que islandComplete tenha o tamanho correto e não seja resetado
        if (ProgessManager.Instance.islandComplete == null || ProgessManager.Instance.islandComplete.Length == 0)
        {
            int totalFases = islands.Length / 2;
            ProgessManager.Instance.islandComplete = new bool[totalFases];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveToNextState();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveToPreviousState();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextScene();
        }
    }

    void MoveToNextState()
    {
        int nextState = ProgessManager.Instance.currentIsland + 1;
        int phaseIndex = ProgessManager.Instance.currentIsland / 2;

        if (nextState < islands.Length && 
            (ProgessManager.Instance.currentIsland % 2 == 0 || ProgessManager.Instance.islandComplete[phaseIndex]))
        {
            ProgessManager.Instance.currentIsland = nextState;
            StartCoroutine(MoveToPosition(islands[nextState].transform.position));
        }
    }

    void MoveToPreviousState()
    {
        int previousState = ProgessManager.Instance.currentIsland - 1;
        int phaseIndex = previousState / 2;

        if (previousState >= 0 && 
            (ProgessManager.Instance.currentIsland % 2 == 1 || !ProgessManager.Instance.islandComplete[phaseIndex]))
        {
            ProgessManager.Instance.currentIsland = previousState;
            StartCoroutine(MoveToPosition(islands[previousState].transform.position));
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void LoadNextScene()
    {
        int phaseIndex = (ProgessManager.Instance.currentIsland / 2) + 1;
        string sceneName = "Fase" + phaseIndex;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log($"A cena {sceneName} não existe ou não está no Build Settings!");
        }
    }
}