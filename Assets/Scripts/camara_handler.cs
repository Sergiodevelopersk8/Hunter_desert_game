using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara_handler : MonoBehaviour
{
    public GameObject min;
    public GameObject max;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject billy = GameObject.Find("Billy");//busco al objeto billy
    
    if((billy.transform.position.x > max.transform.position.x) && max.transform.position.x < GameObject.FindGameObjectWithTag("Nivel").GetComponent<level_handler>().max.transform.position.x)//si la posicion en x de billy es mayor a ala posicion en x de el maximo (limite camara)
        {

            transform.position += new Vector3(0.02f, 0, 0); //aumenta la camara en 5x



        }
    
    }
}
