using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GcFase1 : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeRemaining = 60f;
    private bool isCounting = true;

    public string[] Enunciado;
    public string[] Resposta;

    string respostaCorreta, respostaPlayer;

    public TextMeshProUGUI BotaoUm, BotaoDois, BotaoTres, BotaoQuatro, TextEnunciado;
    private string[] RespostaSplit;

    private int perguntaX;

    public GameObject[] pecas, encaixes;
    private int acertos;

    private Vector3[] posicoesIniciais;

    void Start()
    {
        StartCoroutine(TimerCountdown());
        acertos = -1;

        posicoesIniciais = new Vector3[pecas.Length];
        for (int i = 0; i < pecas.Length; i++)
        {
            posicoesIniciais[i] = pecas[i].transform.position;
        }
        

        ShuffleArraysDouble(Enunciado, Resposta);
        MudarBotoes();
        perguntaX = 0;
    }

    private void AcrescimoPontos(int indice)
    {
        // Verifique se o índice está dentro dos limites dos arrays
        if (indice >= 0 && indice < pecas.Length && indice < encaixes.Length)
        {
            // Obtenha a peça e o encaixe correspondente
            GameObject peca = pecas[indice];
            GameObject encaixe = encaixes[indice];

            // Inicie a animação de movimento suave
            StartCoroutine(MoverComAnimacao(peca, encaixe.transform.position));
        }

        if(indice == 5)
            SceneManager.LoadScene("Inicial");
    }

    // Função de animação suave para mover a peça para o encaixe
    IEnumerator MoverComAnimacao(GameObject peca, Vector3 destino)
    {
        float tempoDeMovimento = 1f; // Tempo que a animação vai durar (ajuste conforme necessário)
        float tempoAtual = 0f;
        Vector3 posicaoInicial = peca.transform.position;

        // Enquanto o tempo de animação não acabar, mova a peça suavemente
        while (tempoAtual < tempoDeMovimento)
        {
            tempoAtual += Time.deltaTime; // Aumenta o tempo de animação
            peca.transform.position = Vector3.Lerp(posicaoInicial, destino, tempoAtual / tempoDeMovimento); // Movimento suave
            yield return null; // Aguarda o próximo quadro
        }

        // Garantir que a peça chegue exatamente ao destino
        peca.transform.position = destino;
    }

    private void ShuffleArray(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]); // Troca os elementos
        }
    }

    private void ShuffleArraysDouble(string[] array1, string[] array2)
    {
        if (array1.Length != array2.Length)
        {
            Debug.LogError("Os arrays precisam ter o mesmo tamanho!");
            return;
        }

        int length = array1.Length;
        int[] indices = new int[length];

        // Inicializa o array de índices sequencialmente
        for (int i = 0; i < length; i++)
        {
            indices[i] = i;
        }

        // Embaralha os índices usando o algoritmo Fisher-Yates
        for (int i = length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]); // Troca os índices
        }

        // Reorganiza os elementos dos arrays usando os índices embaralhados
        string[] shuffledArray1 = new string[length];
        string[] shuffledArray2 = new string[length];

        for (int i = 0; i < length; i++)
        {
            shuffledArray1[i] = array1[indices[i]];
            shuffledArray2[i] = array2[indices[i]];
        }

        // Copia os arrays embaralhados de volta para os originais
        for (int i = 0; i < length; i++)
        {
            array1[i] = shuffledArray1[i];
            array2[i] = shuffledArray2[i];
        }
    }

    private void validarResposta()
    {
        // Verifique se a resposta do jogador corresponde à resposta correta
        if (respostaPlayer.Equals(respostaCorreta))
        {
            acertos++;
            AcrescimoPontos(acertos);
            // Se a resposta estiver correta
            Debug.Log("Resposta correta!");
            // Adicione lógica para premiar o jogador ou passar para a próxima pergunta
        }
        else
        {
            // Se a resposta estiver incorreta
            Debug.Log("Resposta incorreta.");
            // Adicione lógica para penalizar o jogador ou dar outra chance
        }
    }

    private void MudarBotoes()
    {
        // Divide a primeira string do array Resposta
        RespostaSplit = Resposta[perguntaX].Split('?');
        TextEnunciado.text = Enunciado[perguntaX];

        // Variável para armazenar a resposta correta
        respostaCorreta = RespostaSplit[0];

        // Embaralha as respostas
        ShuffleArray(RespostaSplit);

        // Atualiza os botões com as respostas
        BotaoUm.text = RespostaSplit[0];
        BotaoDois.text = RespostaSplit[1];
        BotaoTres.text = RespostaSplit[2];
        BotaoQuatro.text = RespostaSplit[3];

        Debug.Log("RESPOSTA CORRETA " + respostaCorreta);
    }


    public void ClicouNoObjeto(GameObject botaoClicado)
    {
        Transform canvasTransform = botaoClicado.transform.Find("Canvas");
        TextMeshProUGUI RespostaDoPlayer = canvasTransform.GetComponentInChildren<TextMeshProUGUI>();

        respostaPlayer = RespostaDoPlayer.text;
        validarResposta();

        perguntaX++;
        // Verifique se o índice excedeu o tamanho do array antes de incrementar
        if (perguntaX >= Enunciado.Length)
        {
            // Reembaralha os arrays
            ShuffleArraysDouble(Enunciado, Resposta);
            perguntaX = 0; // Reinicia o índice
        }

        MudarBotoes();
    }

    IEnumerator TimerCountdown()
    {
        while (timeRemaining > 0 && isCounting)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            AtualizarTimer();

            // Se o tempo acabar, reseta os pontos e as peças
            if (timeRemaining <= 0)
            {
                ResetarFase();
            }
        }
    }

    private void ResetarFase()
    {
        // Resetando os pontos
        acertos = -1;

        // Resetando as peças para suas posições iniciais
        for (int i = 0; i < pecas.Length; i++)
        {
            pecas[i].transform.position = posicoesIniciais[i];
        }

        StopCoroutine(TimerCountdown());

        // Reiniciando o tempo
        timeRemaining = 60f;
        isCounting = true;
        StartCoroutine(TimerCountdown());
    }

    void AtualizarTimer()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
