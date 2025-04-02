using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgessManager : MonoBehaviour
{
    public static ProgessManager Instance { get; private set; }

    public int currentIsland = 0; // Índice da ilha atual
    public bool[] islandComplete; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void markComplete(){
        int phaseIndex = currentIsland / 2;
        if (phaseIndex < islandComplete.Length)
        {
            islandComplete[phaseIndex] = true;
        }

        Debug.Log("Completa: " + phaseIndex);
    }

    private void Update() {
        Debug.Log(currentIsland);
    }

}
