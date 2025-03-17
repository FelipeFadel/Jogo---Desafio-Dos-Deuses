using UnityEngine;
using TMPro;

public class BotoesFaseUm : MonoBehaviour
{
    private GcFase1 gcFase1;
    private TextMeshProUGUI textMeshPro;
    private Color originalColor;
    private Color highlightColor = new Color(205f / 255f, 106f / 255f, 43f / 255f); // #CD6A2B

    void Start()
    {
        gcFase1 = FindObjectOfType<GcFase1>(); // Encontra o script GcFase1 na cena
        
        // Encontra o TextMeshPro dentro do objeto filho do Canvas
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color; // Salva a cor original do texto
        }
    }

    void OnMouseEnter()
    {
        // Debug.Log("Mouse sobre: " + gameObject.name);
        if (textMeshPro != null)
        {
            textMeshPro.color = highlightColor; // Muda a cor do texto para laranja
        }
    }

    void OnMouseExit()
    {
        // Debug.Log("Mouse saiu de: " + gameObject.name);
        if (textMeshPro != null)
        {
            textMeshPro.color = originalColor; // Restaura a cor original do texto
        }
    }

    void OnMouseDown()
    {
        // Debug.Log("Clicou em: " + gameObject.name);
        gcFase1.ClicouNoObjeto(gameObject); // Chame aqui a função desejada no GcFase1
    }
}
