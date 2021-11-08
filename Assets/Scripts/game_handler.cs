using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_handler : MonoBehaviour
{
 
    public List<GameObject> indice_objetos;
    public List<GameObject> spawns;
    public List<int> vidas_j;
    public List<int> puntos_j;
    public float offset_x_lifes;
    
    // Start is called before the first frame update
    void Start()
    {

        spawn_j1();
        actualizar_puntos(1);


    }
    
    void spawn_j1()
    {
        for (int i = 0; i < vidas_j[0]; i++)//recorro las vidas del jugador 
        {
            GameObject newVida = Instantiate(indice_objetos[4], GameObject.Find("corchete1").transform); //instanciar una nueva vida y agregar como al hijo al corchete correspondiente

            newVida.GetComponent<RectTransform>().position += new Vector3((i) * offset_x_lifes, 0, 0);//posiciona vidas segun el offset (elemento i)
                }
        GameObject newBilly = Instantiate(indice_objetos[0], spawns[0].transform.position, Quaternion.identity);
        newBilly.name = "Billy";

    }

    void actualizar_puntos(int n_jugador)
    {
        string numero_corchete_jugador = "corchete" + n_jugador.ToString();

        GameObject.Find(numero_corchete_jugador).transform.Find("txt_puntos").GetComponent<Text>().text = "$" + puntos_j[n_jugador-1].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
