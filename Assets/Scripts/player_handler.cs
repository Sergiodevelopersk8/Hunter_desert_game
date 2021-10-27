using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_handler : MonoBehaviour
{
    // Start is called before the first frame update
    /*clase 12 video 1*/
    Vector2 velocidad;
    bool is_grounded = true;
    public float vel_desp;
    public float vel_salto;
    public GameObject spr1;
    public GameObject bala;
    public GameObject spr2;
    private Vector2 pos_min;
    private Vector2 pos_max;
    enum estados { idle, walking, jump, dead }
    estados estado_actual = estados.idle;

    void Start()
    {

        pos_min = GameObject.Find("min").transform.position;
        pos_max = GameObject.Find("max").transform.position;

      


    }

    // Update is called once per frame
    void Update()
    {

        Vector3 posicion = transform.position;
        /*creamos una variable para copiar la posicion del transform provisoriamente */

        if (Input.GetKeyDown(KeyCode.A))
        {

            estado_actual = estados.walking;

            velocidad.x = -vel_desp;
            if (!spr1.GetComponent<SpriteRenderer>().flipX ) 
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


        if (Input.GetKeyDown(KeyCode.D))
        {
            //velocidad.x = 0.5f;
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

            if(is_grounded)
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

        if (Input.GetKeyDown(KeyCode.W))
        {

        }

       if (Input.GetKeyDown(KeyCode.S))
        {

        }

        if (Input.GetKeyUp(KeyCode.C) && is_grounded)//salto
        {
            GetComponent<Animator>().SetInteger("estado", 2);//cambio estado uno animacion walk

            velocidad.y += vel_salto;
            is_grounded = false;


            if(!spr1.GetComponent<SpriteRenderer>().flipX)//si esta mirando a la derecha
            {
                spr2.transform.position += new Vector3(-0.09f, 0, 0);
            }

            else
            {
                spr2.transform.position += new Vector3(0.09f, 0, 0);

            }


        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject newBala = Instantiate(bala, transform.position, Quaternion.identity);
        }



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

        if (collision.gameObject.tag == "Suelo") {

            if (!is_grounded) 
            {
                is_grounded = true;
                velocidad.y = 0;

                if(velocidad.x !=0)
                {
                    GetComponent<Animator>().SetInteger("estado", 1);//cambio estado uno animacion walk

                }
                else 
                {
                    GetComponent<Animator>().SetInteger("estado", 0);//cambio estado uno animacion walk


                }


                if (!spr1.GetComponent<SpriteRenderer>().flipX)//si esta mirando a la derecha
                {

                    spr2.transform.position += new Vector3(0.09f, 0, 0);

                }

                else
                {
                    spr2.transform.position += new Vector3(-0.09f, 0, 0);



                }




            }

        }



    }

}
