using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;

public class GcFase1 : MonoBehaviour
{
    public TextMeshProUGUI timerText, apolloFala;
    private float timeRemaining = 60f;
    private bool isCounting = true;

    public GameObject[] apolloScreen;
    public GameObject[] NoPauseElements;
    public string[] EnunciadoUm;
    public string[] EnunciadoDois;
    public string[] EnunciadoTres;
    public string[] RespostaUm;
    public string[] RespostaDois;
    public string[] RespostaTres;

    string respostaCorreta, respostaPlayer;

    public TextMeshProUGUI BotaoUm, BotaoDois, BotaoTres, BotaoQuatro, TextEnunciado;
    private string[] RespostaSplit;

    private int perguntaX;

    public GameObject[] pecas, encaixes;
    private int acertos, currentNivel, erros;

    private string respostaPop;

    private Vector3[] posicoesIniciais;

    private List<string> enunciadosUmList = new List<string>();
    private List<string> respostasUmList = new List<string>();
    private List<string> enunciadosDoisList = new List<string>();
    private List<string> respostasDoisList = new List<string>();
    private List<string> enunciadosTresList = new List<string>();
    private List<string> respostasTresList = new List<string>();

    [System.Serializable]
    public class PlayerInfo
    {
        public string tipo; // "acerto" ou "erro"
        public float tempo; // Tempo restante no momento da resposta
    }

    [System.Serializable]
    public class PlayerData
    {
        public int acertos;  // Add this field for acertos
        public int erros;    // Add this field for erros
        public float tempoRestante; // Add this field for tempoRestante
        public List<PlayerInfo> historico = new List<PlayerInfo>(); // Keep the historico list
    }

    private PlayerData playerData = new PlayerData();
    private string caminhoArquivo;

    void Awake()
    {
        caminhoArquivo = Path.Combine(Application.persistentDataPath, "player_data.json");

    }

    private void RegistrarAcao(bool isAcerto)
    {
        PlayerInfo acao = new PlayerInfo
        {
            tipo = isAcerto ? "acerto" : "erro",
            tempo = timeRemaining
        };

        playerData.historico.Add(acao);
        SalvarDados();
    }

    void SalvarDados()
    {
        PlayerData playerData = new PlayerData
        {
            acertos = acertos,
            erros = erros,
            tempoRestante = timeRemaining
        };

        string json = JsonUtility.ToJson(playerData, true);

        // Salvar o JSON no caminho especificado
        File.WriteAllText(caminhoArquivo, json);

        Debug.Log("Dados salvos em: " + caminhoArquivo);
    }

    private void LimparListas()
    {
        enunciadosUmList.Clear();
        respostasUmList.Clear();
        enunciadosDoisList.Clear();
        respostasDoisList.Clear();
        enunciadosTresList.Clear();
        respostasTresList.Clear();
    }

    void MostrarHistorico()
    {
        foreach (var acao in playerData.historico)
        {
            Debug.Log($"Tipo: {acao.tipo}, Tempo: {acao.tempo}");
        }
    }

    private void encherListas()
    {
        // Preenchendo a pilha de enunciados
        PreencherPilha(enunciadosUmList, EnunciadoUm);
        PreencherPilha(enunciadosDoisList, EnunciadoDois);
        PreencherPilha(enunciadosTresList, EnunciadoTres);

        // Preenchendo a pilha de respostas
        PreencherPilha(respostasUmList, RespostaUm);
        PreencherPilha(respostasDoisList, RespostaDois);
        PreencherPilha(respostasTresList, RespostaTres);
    }

    private void Start()
    {

        StartCoroutine(TimerCountdown());
        acertos = -1;
        erros = 0;
        currentNivel = 1;

        posicoesIniciais = new Vector3[pecas.Length];
        for (int i = 0; i < pecas.Length; i++)
        {
            posicoesIniciais[i] = pecas[i].transform.position;
        }

        LimparListas();
        encherListas();

        MudarBotoes();
        perguntaX = 0;
    }

    private void PreencherPilha(List<string> pilha, string[] array)
    {
        for (int i = array.Length - 1; i >= 0; i--) // Empilhando na ordem correta
        {
            pilha.Add(array[i]);
        }
    }

    private void AcrescimoPontos(int indice, bool isAcerto)
    {
        if (indice >= 0 && indice < pecas.Length && indice < encaixes.Length)
        {
            Debug.Log("sdgwrogwnrogu");
            GameObject peca = pecas[indice];
            GameObject encaixe = encaixes[indice];

            if (isAcerto)
            {
                StartCoroutine(MoverComAnimacao(peca, encaixe.transform.position));
            }
            else
            {
                StartCoroutine(MoverComAnimacao(peca, posicoesIniciais[indice]));
            }
        }
    }

    IEnumerator MoverComAnimacao(GameObject peca, Vector3 destino)
    {
        float tempoDeMovimento = 1f;
        float tempoAtual = 0f;
        Vector3 posicaoInicial = peca.transform.position;

        while (tempoAtual < tempoDeMovimento)
        {
            tempoAtual += Time.deltaTime;
            peca.transform.position = Vector3.Lerp(posicaoInicial, destino, tempoAtual / tempoDeMovimento);
            yield return null;
        }

        peca.transform.position = destino;
    }

    private void ShuffleArray(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
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

        for (int i = 0; i < length; i++)
        {
            indices[i] = i;
        }

        for (int i = length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        string[] shuffledArray1 = new string[length];
        string[] shuffledArray2 = new string[length];

        for (int i = 0; i < length; i++)
        {
            shuffledArray1[i] = array1[indices[i]];
            shuffledArray2[i] = array2[indices[i]];
        }

        for (int i = 0; i < length; i++)
        {
            array1[i] = shuffledArray1[i];
            array2[i] = shuffledArray2[i];
        }
    }

    private void validarResposta()
    {
        if (respostaPlayer.Equals(respostaCorreta))
        {
            acertos++;

            if(acertos > 4){
                ProgessManager.Instance.markComplete();
                SceneManager.LoadScene("FasesHub");
            }

            AcrescimoPontos(acertos, true);
            timeRemaining += 5;
            RegistrarAcao(true);
            Debug.Log("acerto   " + acertos);
        }
        else
        {
            if(currentNivel == 1)
            {
                enunciadosUmList.Insert(0, TextEnunciado.text);
                respostasUmList.Insert(0, respostaPop);
            }
            erros += 1;
            AcrescimoPontos(acertos, false);

            switch(erros){
                case 1:
                    apolloFala.text = "Você ja possui um erro, quando chegar a cinco terá que tentar novamente...";
                    PauseMenu.PauseAnd(apolloScreen, NoPauseElements, 1);
                break;
                case 2:
                    apolloFala.text = "Você ja possui dois erros, quando chegar a cinco terá que tentar novamente...";
                    PauseMenu.PauseAnd(apolloScreen, NoPauseElements, 1);
                break;
                case 3:
                    apolloFala.text = "Você ja possui três erros erro, quando chegar a cinco terá que tentar novamente...";
                    PauseMenu.PauseAnd(apolloScreen, NoPauseElements, 1);
                break;
                case 4:
                    apolloFala.text = "Você ja possui quatro erros, mais um e terá que tentar novamente...";
                    PauseMenu.PauseAnd(apolloScreen, NoPauseElements, 1);
                break;
                case 5:
                    apolloFala.text = "Vamos tentar novamente mortal...";
                    ResetarFase();
                break;

            }
            if(acertos > -1) {acertos--;}
            RegistrarAcao(false);
            Debug.Log("Erro   " + acertos);
        }
    }

    private void MudarBotoes()
    {
        if(enunciadosUmList.Count == 0)
            currentNivel = 2;
        if(enunciadosDoisList.Count == 0)
            currentNivel = 3;
        if(enunciadosTresList.Count == 0)
            currentNivel = 4;
        
        if (currentNivel == 1)
        {
            int lastIndex = respostasUmList.Count - 1;
            respostaPop = respostasUmList[lastIndex];
            respostasUmList.RemoveAt(lastIndex);

            RespostaSplit = respostaPop.Split('?');
            TextEnunciado.text = enunciadosUmList[lastIndex];
            enunciadosUmList.RemoveAt(lastIndex);

            respostaCorreta = RespostaSplit[0];

            ShuffleArray(RespostaSplit);

            BotaoUm.text = RespostaSplit[0];
            BotaoDois.text = RespostaSplit[1];
            BotaoTres.text = RespostaSplit[2];
            BotaoQuatro.text = RespostaSplit[3];
        }

        if (currentNivel == 2)
        {
            int lastIndex = respostasDoisList.Count - 1;
            respostaPop = respostasDoisList[lastIndex];
            respostasDoisList.RemoveAt(lastIndex);

            RespostaSplit = respostaPop.Split('?');
            TextEnunciado.text = enunciadosDoisList[lastIndex];
            enunciadosDoisList.RemoveAt(lastIndex);

            respostaCorreta = RespostaSplit[0];

            ShuffleArray(RespostaSplit);

            BotaoUm.text = RespostaSplit[0];
            BotaoDois.text = RespostaSplit[1];
            BotaoTres.text = RespostaSplit[2];
            BotaoQuatro.text = RespostaSplit[3];
        }

        if (currentNivel == 3)
        {
            int lastIndex = respostasTresList.Count - 1;
            respostaPop = respostasTresList[lastIndex];
            respostasTresList.RemoveAt(lastIndex);

            RespostaSplit = respostaPop.Split('?');
            TextEnunciado.text = enunciadosTresList[lastIndex];
            enunciadosTresList.RemoveAt(lastIndex);

            respostaCorreta = RespostaSplit[0];

            ShuffleArray(RespostaSplit);

            BotaoUm.text = RespostaSplit[0];
            BotaoDois.text = RespostaSplit[1];
            BotaoTres.text = RespostaSplit[2];
            BotaoQuatro.text = RespostaSplit[3];
        }

        if(currentNivel == 4)
            Debug.Log("asffffffffffffffffffff");
    }

    public void ClicouNoObjeto(GameObject botaoClicado)
    {
        Transform canvasTransform = botaoClicado.transform.Find("Canvas");
        TextMeshProUGUI RespostaDoPlayer = canvasTransform.GetComponentInChildren<TextMeshProUGUI>();

        respostaPlayer = RespostaDoPlayer.text;
        validarResposta();

        perguntaX++;

        if (perguntaX >= EnunciadoUm.Length)
        {
            ShuffleArraysDouble(EnunciadoUm, RespostaUm);
            perguntaX = 0;
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

            if (timeRemaining <= 0)
            {
                apolloFala.text = "O seu tempo acabou mortal...";
                ResetarFase();
            }
        }
    }

    private void ResetarFase()
    {
        MostrarHistorico();

        erros = 0;
        acertos = 0;

        PlayerPrefs.DeleteKey("LastPhaseIndex");
        PlayerPrefs.Save();

        SceneManager.LoadScene("FasesHub");
    }

    void AtualizarTimer()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}