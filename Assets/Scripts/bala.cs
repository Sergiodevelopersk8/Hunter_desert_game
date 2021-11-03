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

  public  void asignar_velocidad(float angulo)
    {
        /*cos ang = ady / hipotenusa   a = h*cos 45 */
        velocidad.x = vel_desp * Mathf.Cos(deg2rad(angulo)); /*Adyacente  = hipotenusa * Cos angulo  */
        velocidad.y = vel_desp * Mathf.Sin(deg2rad(angulo));   /*Opuesto  = hipotenusa * Sin(seno) angulo  */
    
    }

   public float deg2rad(float angulo)
    {
        return angulo * 3.14f /180.0f;
    }

}
