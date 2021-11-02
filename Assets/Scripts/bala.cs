using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{

    Vector2 velocidad;
    public float vel_desp;


    // Start is called before the first frame update
    void Start()
    {

        velocidad.x = vel_desp;

    }

    // Update is called once per frame
    void Update()
    {
 
    }


    private void FixedUpdate()
    {
        

        GetComponent<Rigidbody2D>().position += velocidad * Time.deltaTime;

    }



    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
