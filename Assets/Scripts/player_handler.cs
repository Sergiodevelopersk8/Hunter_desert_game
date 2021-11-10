using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_handler : MonoBehaviour
{
    // Start is called before the first frame update
    /*clase 12 video 1*/

    Vector2 velocidad;
    bool is_grounded = false;
    bool plataforma = false;
    bool cooldown = true;
    public float vel_desp;
    public float vel_salto;
    public GameObject spr1;
    public GameObject bala;
    public GameObject spr2;
    public GameObject spawns;
    private Vector2 pos_min;
    private Vector2 pos_max;
    enum estados { idle, walking, jump, dead }
    estados estado_actual = estados.idle;

    bool[] teclas = { false,false,false,false }; //0 arriba, 1 abajo, 2 izquierda, 3 derecha
    enum direcciones  {derecha, izquierda, arriba, abajo, derarr, derab, izqarr, izqab};
    direcciones direccion = direcciones.derecha;

    void Start()
    {



        pos_min = GameObject.Find("min").transform.position;
        pos_max = GameObject.Find("max").transform.position;

        GetComponent<Animator>().SetInteger("estado", 2);
        spr2.transform.position += new Vector3(-0.09f, 0, 0);


    }

    // Update is called once per frame
    void Update()
    {

        if (estado_actual != estados.dead) 
        { 


        Vector3 posicion = transform.position;
        /*creamos una variable para copiar la posicion del transform provisoriamente */

        if (Input.GetKeyDown(KeyCode.A) && GetComponent<Animator>().GetInteger("estado") != 3)
        {
            teclas[2] = true;
            estado_actual = estados.walking;

            velocidad.x = -vel_desp;
            if (!spr1.GetComponent<SpriteRenderer>().flipX)
            {
                spr1.GetComponent<SpriteRenderer>().flipX = true;
                spr2.GetComponent<SpriteRenderer>().flipX = true;
                if (is_grounded)
                {
                    spr2.transform.position += new Vector3(-0.07f, 0, 0);
                }

                else
                {
                    spr2.transform.position += new Vector3(0.09f, 0, 0);

                }



            }



            if (is_grounded)
            {
                GetComponent<Animator>().SetInteger("estado", 1);//cambio estado uno animacion walk

            }

            /*calculamos  sobre esta variable (restamos 0.5 a la x para que se mueva a la izquierda)*/


        }


        if (Input.GetKeyDown(KeyCode.D) && GetComponent<Animator>().GetInteger("estado") != 3)
        {

            teclas[3] = true;
            velocidad.x = vel_desp;
            if (spr1.GetComponent<SpriteRenderer>().flipX)
            {
                spr1.GetComponent<SpriteRenderer>().flipX = false;
                spr2.GetComponent<SpriteRenderer>().flipX = false;

                if (is_grounded)
                {
                    spr2.transform.position += new Vector3(+0.07f, 0, 0);

                }

                else
                {
                    spr2.transform.position += new Vector3(-0.09f, 0, 0);
                }
            }

            if (is_grounded)
            {
                GetComponent<Animator>().SetInteger("estado", 1);//cambio estado uno animacion walk

            }


        }

        if (Input.GetKeyUp(KeyCode.A) && velocidad.x < 0 || Input.GetKeyUp(KeyCode.D) && velocidad.x > 0)
        {
            velocidad.x = 0.0f;

            if (is_grounded)
            {
                GetComponent<Animator>().SetInteger("estado", 0);//cambio estado uno animacion walk

            }

        }

        if (Input.GetKeyDown(KeyCode.S) && is_grounded)
        {
            GetComponent<Animator>().SetInteger("estado", 3);
            spr2.transform.position += new Vector3(0, 0.0911f, 0);
            transform.position += new Vector3(0, -0.14f, 0);
        }

        if (Input.GetKeyUp(KeyCode.S) && GetComponent<Animator>().GetInteger("estado") == 3)
        {
            GetComponent<Animator>().SetInteger("estado", 0);
            spr2.transform.position += new Vector3(0, -0.0911f, 0);
            transform.position += new Vector3(0, 0.14f, 0);

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            teclas[0] = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            teclas[1] = true; Debug.Log("tecla s presionada" + teclas);

        }


        if (Input.GetKeyUp(KeyCode.A))
        {
            teclas[2] = false;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            teclas[3] = false;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            teclas[0] = false;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            teclas[1] = false;
            Debug.Log("tecla s soltada" + teclas);
        }

        if (Input.GetKeyUp(KeyCode.C) && is_grounded)//salto
        {
            if (GetComponent<Animator>().GetInteger("estado") != 3)
            {
                GetComponent<Animator>().SetInteger("estado", 2);//cambio estado uno animacion walk

                velocidad.y += vel_salto;
                is_grounded = false;


                if (!spr1.GetComponent<SpriteRenderer>().flipX)//si esta mirando a la derecha
                {
                    spr2.transform.position += new Vector3(-0.09f, 0, 0);
                }

                else
                {
                    spr2.transform.position += new Vector3(0.09f, 0, 0);

                }

            }

            else if (plataforma) /*si esta agachado voy a qquerer ver si esta en la plataforma para bajarlo*/
            {
                plataforma = false;
                GetComponent<Animator>().SetInteger("estado", 2);
                spr2.transform.position += new Vector3(0, -0.0911f, 0);
                is_grounded = false;

                if (!spr1.GetComponent<SpriteRenderer>().flipX)//si esta mirando a la derecha
                {
                    spr2.transform.position += new Vector3(-0.09f, 0, 0);
                }

                else
                {
                    spr2.transform.position += new Vector3(0.09f, 0, 0);

                }


            }
        }


        if (Input.GetKeyDown(KeyCode.X) && cooldown)
        {
            cooldown = false;
            chechar_teclas();
            Invoke("habilitar_cooldown", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.P))/*TESTEAR MUERTE*/
        {
            muerte();
        }

    }
    }


    void muerte()
    {
        GetComponent<Animator>().SetInteger("estado", 5);
        transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 7);
        estado_actual = estados.dead;
        
        Destroy(gameObject,2.0f);
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
        if (GetComponent<Rigidbody2D>().position.x > pos_max.x )
        {
            GetComponent<Rigidbody2D>().position = new Vector2(pos_max.x, GetComponent<Rigidbody2D>().position.y); 
        }

        else if(GetComponent<Rigidbody2D>().position.x < pos_min.x)
        {
            GetComponent<Rigidbody2D>().position = new Vector2(pos_min.x, GetComponent<Rigidbody2D>().position.y);

        }
    }


    void mover_player()
    {
        velocidad.x = vel_desp;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Suelo" || (collision.gameObject.tag =="Obstaculo" && velocidad.y < 0)|| (collision.gameObject.tag == "Plataforma" && velocidad.y < 0))
        {

            if (!is_grounded)
            {
                is_grounded = true;
                velocidad.y = 0;

                if (velocidad.x != 0)
                {
                    GetComponent<Animator>().SetInteger("estado", 1);//cambio estado uno animacion walk

                }
                else
                {
                    GetComponent<Animator>().SetInteger("estado", 0);//cambio estado uno animacion idle

                    transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 0);
                }


                if (!spr1.GetComponent<SpriteRenderer>().flipX)//si esta mirando a la derecha
                {

                    spr2.transform.position += new Vector3(0.09f, 0, 0);

                }

                else
                {
                    spr2.transform.position += new Vector3(-0.09f, 0, 0);



                }

                if (collision.gameObject.tag == "Plataforma")
                {
                    plataforma = true;
                }
                else
                {
                    plataforma = false;
                }

                if(estado_actual == estados.dead)
                {
                    velocidad.x = 0;
                }

            }

        }

        else if (collision.gameObject.tag == "Plataforma" && velocidad.y > 0 && teclas[0] == true) 
        {
            GetComponent<Animator>().SetInteger("estado", 4);
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 6);
            velocidad.y += vel_salto * 1.1f;

        }


        if(collision.gameObject.tag == "Obstaculo")  //si coliciona con obstaculo
        {

            Debug.Log("colision con obstaculo");
            velocidad.x = 0; //velocidad en x es 0
            if (
            GetComponent<Animator>().GetInteger("estado")==1) //SI estaba caminando
            {
                GetComponent<Animator>().SetInteger("estado", 0); //se pone en reposo

            }
        }

    }


    private void OnCollisionExit2D(Collision2D   collision) //al salir de  colision
    {
       if((collision.gameObject.tag == "Plataforma" || collision.gameObject.tag == "Obstaculo"  ) &&  velocidad.y <= 0)//chequea si el objeto del cual sali es una plataforma  u obstaculo y segun eso se habilita la caida

        {
            is_grounded = false;
            GetComponent<Animator>().SetInteger("estado", 2); //estados callendo
            if (!spr1.GetComponent<SpriteRenderer>().flipX)//si esta mirando a la derecha
            {

                spr2.transform.position += new Vector3(-0.09f, 0, 0);

            }

            else
            {
                spr2.transform.position += new Vector3(0.09f, 0, 0);



            }
        }
    }


    void habilitar_cooldown()
    {
        cooldown = true;
        transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 0);

    }

    void chechar_teclas()
    {
        Vector3 posicion = new Vector3();

        if (teclas[0])
        {
            if (teclas[2])
            {
                direccion = direcciones.izqarr;
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 2);
                //apunta diagonal arriba izquierda
                posicion = spawns.transform.Find("Spawn_IARR ").transform.position;
            }

            else if (teclas[3])
            {
                //apunta diagonal arriba derecha
                direccion = direcciones.derarr;
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 2);
                posicion = spawns.transform.Find("Spawn_DARR").transform.position;
            }

            else
            {
                //apunta arriba
                direccion = direcciones.arriba;
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 4);
                posicion = spawns.transform.Find("Spawn_AR").transform.position;
            }
        }

        else if (teclas[1]) 
        {
            if (teclas[2] ) //izquierda abajo
            {
                direccion = direcciones.izqab;
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 3);
                posicion = spawns.transform.Find("Spawn_IAB").transform.position;
            }
             
            else if (teclas[3])
            {
                //derecha abajo
                direccion = direcciones.derab;
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 3);
                posicion = spawns.transform.Find("Spawn_DAB").transform.position;
            }
            else
            {
                //abajo
                direccion = direcciones.abajo;
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 5);
                posicion = spawns.transform.Find("Spawn_AB").transform.position;
            }
        }

        else if (teclas[2]) //izquierda 
        {
            direccion = direcciones.izquierda;
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 1);
            posicion = spawns.transform.Find("Spawn_I").transform.position;
          
        }
        else if (teclas[3]) //derecha
        {
            direccion = direcciones.derecha;
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 1);
            posicion = spawns.transform.Find("Spawn_D").transform.position;
        }

        else
        {
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 1);
            if (spr1.GetComponent<SpriteRenderer>().flipX)
            {
                posicion = spawns.transform.Find("Spawn_I").transform.position;
                direccion = direcciones.izquierda;
            }
            else
            {
                posicion = spawns.transform.Find("Spawn_D").transform.position;
                direccion = direcciones.derecha;
            }
        }

        check_direccion(posicion);

       
        //newBala.GetComponent<bala>().asignar_velocida_diagonal(false);
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
