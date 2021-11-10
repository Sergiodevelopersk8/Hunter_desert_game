using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1_handler : MonoBehaviour
{
    Vector2 velocidad;
    bool is_grounded = false;
    bool plataforma = false;
    bool cooldown = true;
    public float vel_desp;
    public float vel_salto;
    public GameObject bala;
    public GameObject spawns;
    public GameObject laser_spawn;
    private Vector2 pos_min;
    private Vector2 pos_max;
    enum estados { idle, walking, jump, dead }
    estados estado_actual = estados.walking;
    enum direcciones { derecha, izquierda, arriba, abajo, derarr, derab, izqarr, izqab };
    direcciones direccion = direcciones.izquierda;


    // Start is called before the first frame update
    void Start()
    {


        pos_min = GameObject.Find("min").transform.position;
        pos_max = GameObject.Find("max").transform.position;

        GetComponent<Animator>().SetInteger("estado", 7);


    }

    // Update is called once per frame
    void Update()
    {
        if (estado_actual != estados.dead)
        {
        
            if(estado_actual == estados.walking && is_grounded) 
         
            {
                if(!GetComponent<SpriteRenderer>().flipX)
                {
                    //velocidad.x = -vel_desp;
                }

                else 
                {
                   // velocidad.x = vel_desp;

                }

            }

        }
        Invoke("disparar", 2.0f);
    }

    void muerte()
    {
        GetComponent<Animator>().SetInteger("estado", 6);
        estado_actual = estados.dead;

        Destroy(gameObject, 2.0f);
    }

    private void FixedUpdate()
    {
        if (!is_grounded)
        {
            velocidad += Physics2D.gravity * Time.deltaTime; /*multiplicamos gravedad * tiempo para obtener velocidad (v = a*t)*/

        }
        GetComponent<Rigidbody2D>().position += velocidad * Time.deltaTime;

        check_limites();
    }

    void check_limites()
    {
        if (GetComponent<Rigidbody2D>().position.x > pos_max.x)
        {
            GetComponent<Rigidbody2D>().position = new Vector2(pos_max.x, GetComponent<Rigidbody2D>().position.y);
        }

        else if (GetComponent<Rigidbody2D>().position.x < pos_min.x)
        {
            GetComponent<Rigidbody2D>().position = new Vector2(pos_min.x, GetComponent<Rigidbody2D>().position.y);

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Suelo" || (collision.gameObject.tag == "Plataforma" && velocidad.y < 0))
        {

            if (!is_grounded)
            {
                is_grounded = true;
                velocidad.y = 0;

               
               
                    GetComponent<Animator>().SetInteger("estado", 0);//cambio estado uno animacion walk
                                          
                                
                if (collision.gameObject.tag == "Plataforma")
                {
                    plataforma = true;
                }
                else
                {
                    plataforma = false;
                }

                if (estado_actual == estados.dead)
                {
                    velocidad.x = 0;
                }

            }

        }

        else if (collision.gameObject.tag == "Plataforma" && velocidad.y > 0 )
        {
            Debug.Log("trepando");
            GetComponent<Animator>().SetInteger("estado", 5); //cambio estado a trepar
            velocidad.y += vel_salto * 1.1f;

        }


        if (collision.gameObject.tag == "Obstaculo")  //si coliciona con obstaculo
        {

            velocidad.x = 0; //velocidad en x es 0
         RaycastHit2D col =   Physics2D.Raycast(new Vector2(laser_spawn.transform.position.x, laser_spawn.transform.position.y), new Vector2(0, 1));//creamos un raycast para detectar plataforma superior


            if (col != null && col.collider != null) //si colisiono con algo y no estoy tratando de detectar la nada misma 
            {
            
                if (col.collider.gameObject.tag == "Plataforma")
                {
                    if(is_grounded)
                    saltar();
                }
            }
            
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX; //hago el volteado opuesto
        }



    }

    private void OnCollisionExit2D(Collision2D collision) //al salir de  colision
    {
        if ((collision.gameObject.tag == "Plataforma" || collision.gameObject.tag == "Obstaculo") && velocidad.y <= 0 && !is_grounded)//chequea si el objeto del cual sali es una plataforma  u obstaculo y segun eso se habilita la caida

        {
            is_grounded = false;
            GetComponent<Animator>().SetInteger("estado", 7); //estados callendo
          
        }
    }





    void saltar()
    {
        velocidad.x = 0;
        GetComponent<Animator>().SetInteger("estado", 7); //salta
        velocidad.y += vel_salto;
        is_grounded = false;
    }


      void disparar()
    {
        if(is_grounded)
        {
            int resultado = Random.Range(0, 11);//al ser int el maximo no sera inclusivo esto, sera de 0 a 10
            if(resultado > 5)
            {
                Debug.Log("si entro para disparar");
                //generar bala
                GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        Vector2 distancia = new Vector2(jugador.transform.position.x - Mathf.Abs( transform.position.x), ( jugador.transform.position.y - transform.position.y));
                float angulo = Mathf.Atan2(distancia.y, distancia.x); //utilizo pitagoras (Tang Ang = op / ady)para obtener el angulo al que tendria que apuntar 
                angulo *=  Mathf.Rad2Deg;
                angulo += 360;
                   GameObject newBala = Instantiate(bala, transform.position, Quaternion.identity);
                 newBala.GetComponent<bala>().asignar_velocidad(angulo);

                //Debug.Log(angulo);
                //if (angulo < 0f)
                //{
                //    Debug.Log("si entro por que es menor a 0");
                //  angulo += 360;
                //    GameObject newBala = Instantiate(bala, transform.position, Quaternion.identity);
                //    newBala.GetComponent<bala>().asignar_velocidad(angulo);
                //}
            }
            

        }
        Invoke("disparar", 2.0f);
    }

    void habilitar_cooldown()
    {
        cooldown = true;
        GetComponent<Animator>().SetInteger("estado", 0);

    }


    public void check_direccion(Vector3 posicion)
    {

        GameObject newBala = Instantiate(bala, posicion, Quaternion.identity);

        switch (direccion)
        {
            case direcciones.derecha:

                newBala.GetComponent<bala>().asignar_velocidad(0);
                break;

            case direcciones.izquierda:
                newBala.GetComponent<bala>().asignar_velocidad(180);

                break;

            case direcciones.arriba:
                newBala.GetComponent<bala>().asignar_velocidad(90);

                break;


            case direcciones.abajo:
                newBala.GetComponent<bala>().asignar_velocidad(270);

                break;


            case direcciones.derab:
                newBala.GetComponent<bala>().asignar_velocidad(315);

                break;


            case direcciones.derarr:
                newBala.GetComponent<bala>().asignar_velocidad(45);

                break;


            case direcciones.izqab:
                newBala.GetComponent<bala>().asignar_velocidad(225);

                break;


            case direcciones.izqarr:
                newBala.GetComponent<bala>().asignar_velocidad(135);

                break;



        }
    }



}
