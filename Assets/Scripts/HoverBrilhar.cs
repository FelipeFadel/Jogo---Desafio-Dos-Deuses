using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class HoverBrilhar : MonoBehaviour
{
    public Color hoverColor = Color.yellow;
    public Color clickColor = Color.red;
    private Color originalColor;
    private Renderer rend;

    public string FaseName;
    public Camera mainCamera;
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;

    private bool dir;

    private void Start()
    {
        dir = false;
        rend = GetComponent<Renderer>();
        if (rend.material.HasProperty("_EmissionColor"))
        {
            originalColor = rend.material.GetColor("_EmissionColor");
        }
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out depthOfField);
        }
    }

    private void creditos() // Direita
    {
        if (mainCamera != null)
        {
            mainCamera.transform.rotation = Quaternion.Euler(0, -270, 0);
        }
        ToggleDepthOfField(false);
    }

    private void opcoes() // Esquerda
    {
        if (mainCamera != null)
        {
            mainCamera.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        ToggleDepthOfField(false);
    }

    private void voltar() // Centro
    {
        if (mainCamera != null)
        {
            mainCamera.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        ToggleDepthOfField(true);
    }

    private void ToggleDepthOfField(bool enable)
    {
        if (depthOfField != null)
        {
            depthOfField.active = enable;
        }
    }

    void OnMouseEnter()
    {
        if (rend.material.HasProperty("_EmissionColor"))
        {
            rend.material.SetColor("_EmissionColor", hoverColor);
            rend.material.EnableKeyword("_EMISSION");
        }
    }

    void OnMouseExit()
    {
        if (rend.material.HasProperty("_EmissionColor"))
        {
            rend.material.SetColor("_EmissionColor", originalColor);
        }
    }

    void OnMouseDown()
    {
        if (rend.material.HasProperty("_EmissionColor"))
        {
            rend.material.SetColor("_EmissionColor", clickColor);
        }
        
        if (FaseName == "FasesHub")
            SceneManager.LoadScene(FaseName);
        if (FaseName == "Creditos")
        {
            dir = true;
            creditos();
        }
        if (FaseName == "Opcoes")
        {
            dir = false;
            opcoes();
        }
        if (FaseName == "Voltar")
            voltar();
    }
}