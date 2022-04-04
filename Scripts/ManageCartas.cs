using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ManageCartas : MonoBehaviour
{   
    public GameObject carta;  // Carta a ser descartada
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; // indicadores para cada carta escolhida em cada linha
    private GameObject carta1, carta2;  //gameObjects 1ª e 2ª carta selecionada
    private string linhaCarta1, linhaCarta2;  // linhas das cartas

    bool timerPausado, timerAcionado;  // indicadores do Timer
    float timer;        // variável de tempo
    int numTentativas = 0; // numero de tentativas
    int numAcertos = 0;  // numero de acertos
    AudioSource somOk;  // som de acerto

    int ultimoJogo = 0; // variável que guardará o número de tentativas do jogo anterior
    


    // Start is called before the first frame update
    void Start()
    {
        MostraCarta();
        UpdateTentativas();
        somOk = GetComponent<AudioSource>();
        ultimoJogo = PlayerPrefs.GetInt("Jogadas");
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Jogo Anterior = " + ultimoJogo;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if (timer >1)
            {   
                //define o tempo que as caras ficarão reveladas
                timerPausado = true;
                timerAcionado = false;
                if (carta1.tag == carta2.tag)
                {   
                    //elimina as cartas caso o jogador tenha acertado e toca o som de acerto
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    somOk.Play();
                    if (numAcertos == 13)
                    {
                        // checa se o jogador conseguiu ganhar o jogo, guarda o valor de tentativas e chama cena dos créditos
                        PlayerPrefs.SetInt("Jogadas", numTentativas);
                        SceneManager.LoadScene("Creditos");
                    }

                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }

    }

    /*
    Esse método cria dois vetores, preenche eles chamando a função
    de embaralhar cartas e adiciona as cartas em duas linhas diferentes
    chamando a função de adicionar cartas.    
    */
    void MostraCarta()
    {
        int [] arrayEmbaralhado = criaArrayEmbaralhado();
        int [] arrayEmbaralhado2 = criaArrayEmbaralhado();
        //Instantiate(carta,new Vector3(0,0,0), Quaternion.identity);
        //AddUmaCarta();
        for (int i = 0; i<13; i++)
        {
            // AddUmaCarta(i);
            // AddUmaCarta(i, arrayEmbaralhado[i]);
            AddUmaCarta(0,i, arrayEmbaralhado[i]);
            AddUmaCarta(1,i, arrayEmbaralhado2[i]);
        }
      
    }

    /*
    Esse método cria as cartas e as adiciona em duas linhas diferentes,
    usando o centro da tela como parâmetro de posicionamento.  
    */
    void AddUmaCarta(int linha, int rank, int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (650*escalaCartaOriginal)/110.0f;
        float fatorEscalaY = (945*escalaCartaOriginal)/110.0f;
        Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank-13/2)*fatorEscalaX),centro.transform.position.y+ ((linha-2/2)*fatorEscalaY) ,centro.transform.position.z);
        //Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank-13/2)*fatorEscalaX),centro.transform.position.y ,centro.transform.position.z);
        //Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank-13/2)*1.2f),centro.transform.position.y ,centro.transform.position.z);
        //GameObject c = (GameObject)(Instantiate(carta, new Vector3(0,0,0), Quaternion.identity));
        //GameObject c = (GameObject)(Instantiate(carta, new Vector3(rank*1.5f,0,0), Quaternion.identity));
        GameObject c = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity));
        c.tag = "" + (valor + 1);
        // c.name = "" + valor;
        c.name = "" + linha + "_"+ valor;
        string nomeDaCarta = "";
        string numeroCarta = "";
        /* if (rank ==0)
            numeroCarta = "ace";
        else if (rank == 10)
            numeroCarta = "jack";
        else if (rank == 11)
            numeroCarta = "queen";
        else if (rank == 12)
            numeroCarta = "king";
        else
            numeroCarta = "" + (rank + 1);*/  // eslse if para deck ordenado

        if (valor ==0)
            numeroCarta = "ace";
        else if (valor == 10)
            numeroCarta = "jack";
        else if (valor  == 11)
            numeroCarta = "queen";
        else if (valor == 12)
            numeroCarta = "king";
        else
            numeroCarta = "" + (valor + 1);
        nomeDaCarta = numeroCarta + "_of_clubs";
        Sprite s1 = (Sprite)(Resources.Load<Sprite>(nomeDaCarta));
        print("S1: " + s1);
        // GameObject.Find(""+rank).GetComponent<Tile>().setCartaOriginal(s1);
        // GameObject.Find(""+valor).GetComponent<Tile>().setCartaOriginal(s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().setCartaOriginal(s1);
    }

    /*
    Esse método cria e retorna um vetor embaralhado de valores 0 a 12,
    para servir de parâmetro para cada linha de cartas.
    */
    public int[] criaArrayEmbaralhado()
    {
        int[] novoArray = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12};
        int temp;
        for (int t=0; t<13; t++)
        {
            temp = novoArray[t];
            int r = Random.Range(t, 13);
            novoArray[t] = novoArray[r];
            novoArray[r] = temp;
        }
    return novoArray;
    }


    /*
    Esse método seleciona a carta que foi clicada pelo usuário e
    chama a função que a revela.
    */
    public void CartaSelecionada(GameObject carta)
    {
        if(!primeiraCartaSelecionada)
        {
            string linha = carta.name.Substring(0,1);
            linhaCarta1 = linha;
            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0,1);
            linhaCarta2 = linha;
            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();
            VerificaCartas();
        }
    }


    /*
    Esse método verifica se as cartas selecionadas são iguais
    e chama a função contadora de tentativas.
    */
    public void VerificaCartas()
    {
        disparaTimer();
        numTentativas++;
        UpdateTentativas();
    }

    /*
    Esse método dispara o timer que define o tempo que as
    cartas selecionadas ficarão reveladas
    */
    public void disparaTimer()
    {
        timerPausado = false;
        timerAcionado = true;
    }


    /*
    Esse método atualiza o número de tentativas que o usuário
    ja realizou.
    */
    void UpdateTentativas()
    {

        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas;
    }
}
