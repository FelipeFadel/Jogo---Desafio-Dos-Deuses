using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HexHover : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float hoverHeight = 0.33f;
    private float smoothSpeed = 5f;

    private Renderer hexRenderer;
    public Material normalMaterial;
    public Material hoverMaterial;

    // Arrays de Objetos e Materiais
    public GameObject[] objects;
    public Material[] materials;
    private Material[] originalMaterials;

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;
        hexRenderer = GetComponent<Renderer>();

        // Salva os materiais originais
        originalMaterials = new Material[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            Renderer objRenderer = objects[i].GetComponent<Renderer>();
            if (objRenderer != null)
            {
                originalMaterials[i] = objRenderer.material;
            }
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }

    void OnMouseEnter()
    {
        targetPosition = originalPosition + Vector3.up * hoverHeight;
        FindObjectOfType<MovimentoDeCamera>().FocusOnHexagon(transform);

        StopAllCoroutines();
        StartCoroutine(TransitionMaterial(hexRenderer, normalMaterial, hoverMaterial, 0.5f));
        ApplyMaterials();
    }

    void OnMouseExit()
    {
        targetPosition = originalPosition;
        FindObjectOfType<MovimentoDeCamera>().ResetCamera();

        StopAllCoroutines();
        StartCoroutine(TransitionMaterial(hexRenderer, hoverMaterial, normalMaterial, 0.5f));
        ResetMaterials();
    }

    void OnMouseDown()
    {
        string sceneName = gameObject.name;
        if (SceneExists(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("A cena '" + sceneName + "' não existe. Verifique o nome.");
        }
    }

    void ApplyMaterials()
    {
        if (objects.Length == materials.Length)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Renderer objRenderer = objects[i].GetComponent<Renderer>();
                if (objRenderer != null)
                {
                    StartCoroutine(TransitionMaterial(objRenderer, originalMaterials[i], materials[i], 0.5f));
                }
            }
        }
        else
        {
            Debug.LogWarning("Os arrays de objetos e materiais não têm o mesmo tamanho!");
        }
    }

    void ResetMaterials()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Renderer objRenderer = objects[i].GetComponent<Renderer>();
            if (objRenderer != null)
            {
                StartCoroutine(TransitionMaterial(objRenderer, materials[i], normalMaterial, 0.5f));
            }
        }
    }

    IEnumerator TransitionMaterial(Renderer objRenderer, Material startMaterial, Material endMaterial, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            objRenderer.material.Lerp(startMaterial, endMaterial, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objRenderer.material = endMaterial;
    }

    bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
