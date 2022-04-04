using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{   
    private bool tileRevelada = false; // indica se carta está virada ou não
    public Sprite originalCarta;       // Sprite da carta desejada 
    // public Sprite novaCarta;            // Carta a ser descartada
    public Sprite backCarta;           // Sprite do verso da carta


    // Start is called before the first frame update
    void Start()
    {
        EscondeCarta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
    Esse método imprime que um tile foi clicado
    e revela a carta que foi clicada.    
    */
    public void OnMouseDown()
    {
        print("Você pressionou num Tile");
        /*if(tileRevelada)
            EscondeCarta();
        else
            RevelaCarta();*/   // aqui não se guardava o número de cartas

        GameObject.Find("gameManager").GetComponent<ManageCartas>().CartaSelecionada(gameObject);
    }


    /*
    Esse método serve para deixar as cartas com
    a face de trás virada para cima    
    */
    public void EscondeCarta()
    {
        GetComponent<SpriteRenderer>().sprite = backCarta;
        tileRevelada = false;
    }


    /*
    Esse método serve para deixar as cartas com
    a face da frente virada para cima   
    */
    public void RevelaCarta()
    {
        GetComponent<SpriteRenderer>().sprite = originalCarta;
        tileRevelada = true;
    }


     /*
    Esse método serve para definir a carta original
    como a face da frente   
    */
    public void setCartaOriginal(Sprite novaCarta)
    {
        originalCarta = novaCarta;
    }


}
