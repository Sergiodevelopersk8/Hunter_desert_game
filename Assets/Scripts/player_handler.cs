using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_handler : MonoBehaviour
{
    // Start is called before the first frame update
    /*clase 12 video 1*/
    Vector2 velocidad;
    bool is_grounded = true;
    bool cooldown = true;
    public float vel_desp;
    public float vel_salto;
    public GameObject spr1;
    public GameObject bala;
    public GameObject spr2;
    private Vector2 pos_min;
    private Vector2 pos_max;
    enum estados { idle, walking, jump, dead }
    estados estado_actual = estados.idle;

    bool[] teclas = { false,false,false,false }; //0 arriba, 1 abajo, 2 izquierda, 3 derecha

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
            teclas[2] = true;
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


        if (Input.GetKeyDown(KeyCode.X) && cooldown)
        {
            GameObject newBala = Instantiate(bala, transform.position, Quaternion.identity);
            cooldown = false;
            chechar_teclas();
            Invoke("habilitar_cooldown", 0.5f);
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


    void habilitar_cooldown()
    {
        cooldown = true;
        transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 0);

    }

    void chechar_teclas()
    {
        if (teclas[0])
        {
            if (teclas[2])
            {
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 2);
                //apunta diagonal arriba izquierda
            }

            else if (teclas[3])
            {
                //apunta diagonal arriba derecha
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 2);
            }

            else
            {
                //apunta arriba
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 4);
            }
        }

        else if (teclas[1]) 
        {
            if (teclas[2] ) //abajo
            {
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 3);
            }
             
            else if (teclas[3])
            {
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 3);
            }
            else
            {
                transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 5);
            }
        }

        else if (teclas[2]) //izquierda
        {
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 1);
        }
        else if (teclas[3]) //derecha
        {
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 1);
        }

        else
        {
            transform.Find("Spr_Billy").GetComponent<Animator>().SetInteger("estado", 1);
        }

    }

}
