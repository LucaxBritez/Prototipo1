using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isLeft = false;
    public bool isRight = false;


    public bool grounded;
    public bool permitirSalto = false;
    public bool saltando = false;

    private Animator animator;
    public Rigidbody2D rb;

    public float speedForce;
    public float jumpForce;


    public enum EstadoDeAnimacion
    {
        Quieto = 0,
        Corriendo = 1,
        Saltando = 2,
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ControlarInputsHorizontales();

        ControlarSalto();
        ManejarAnimacionSalto();
        ManejarAnimacionCorrer();

    }

    //El contenido de esta region se utiliza para controlar la accion de salto
    #region Salto
    //Funcional
    public void ControlarSalto()
    {
        //Esta linea sirve para utilizar un gizmo o ray para visualizar el impacto en la parte inferior de John
        Debug.DrawRay(transform.position, Vector3.down * 0.15f, Color.red);

        //Este condicional se encarga de asegurarse que la entidad esta chocando el suelo, y en caso de ser asi, devuelve un grounded true
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.15f))
        {
            grounded = true;
            
        }
        else
        {
            grounded = false;
            
        }

        if (permitirSalto && grounded)
        {
            Jump();
            permitirSalto = false;
        }
    }
    //Funcional
    private void Jump()
    {
        
        rb.AddForce(Vector2.up * jumpForce);
        
    }
    //Trabajar en esto
    private void ManejarAnimacionSalto()
    {
        if (!grounded)
        {
            //animator.SetBool("jumping", true);
            Debug.Log("Entramos a cambiar Salto");
            animator.SetInteger("Estado", (int)EstadoDeAnimacion.Saltando);
            animator.SetBool("jumping",permitirSalto);
            Debug.Log("Llegamos al otro lado");
        }
        if (grounded)
        {
            //Debug.Log("Entramos aca?");
            //animator.SetBool("jumping", false);
            animator.SetInteger("Estado", (int)EstadoDeAnimacion.Quieto);
        }

    }
    #endregion



    //Esta region se encarga del comportamiento de los botones
    //Probablemente puede implementarse el enum a partir de estos metodos
    #region Botones
    public void ClickJump()
    {
       permitirSalto = true;
    }

    
    public void ClickLeft()
    {
        isLeft = true; 
    }

    public void ReleaseLeft()
    {
        isLeft=false;
    }

    public void ClickRight()
    {
        isRight = true; 
    }

    public void ReleaseRight()
    {
        isRight=false;
    }
    #endregion


    //Esta region se encarga del funcionamiento de la accio correr.
    #region Correr
    public void ManejarAnimacionCorrer()
    {
        
        /*Anterior modo de cambiar de animacion usando bools
        animator = GetComponent<Animator>();
        animator.SetBool("running", isLeft||isRight);
        */
        
        //Actual modo de cambiar de animacion en correr utilizando un enum y paremtros int
        //ConttrolarIzquierdaOParar();
        if (isLeft)
        {
            animator.SetInteger("Estado", (int)EstadoDeAnimacion.Corriendo);
            transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
           
        }
        else 
        {
            ControlarDerechaOParar();
        }

        
    }

    public void ControlarDerechaOParar()
    {
        if (isRight)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            animator.SetInteger("Estado", (int)EstadoDeAnimacion.Corriendo);
        }
        else
        {
            animator.SetInteger("Estado", (int)EstadoDeAnimacion.Quieto);
        }
    }
    private void ControlarInputsHorizontales()
    {
        if (isLeft)
        {
            rb.AddForce(new Vector2(-speedForce, 0) * Time.deltaTime);
        }
        if (isRight)
        {
            rb.AddForce(new Vector2(speedForce, 0) * Time.deltaTime);
        }
    }
    #endregion
    // Update is called once per frame
    
}
