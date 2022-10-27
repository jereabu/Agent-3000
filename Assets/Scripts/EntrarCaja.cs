﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class EntrarCaja : MonoBehaviour
{
    public GameObject jugador;
    public GameObject jugador2;
    public FirstPersonController jugadorController;
    public CharacterController controller;
    public GameObject caja;
    [SerializeField] Vector3 posicionjugadorcaja;
    public GameObject camCaja;
    public GameObject camJugador;
    public Text Entrar;
    public Text Salir;



    // Start is called before the first frame update
    void Start()
    {   
        jugador = GameObject.FindWithTag("Player");
        jugador2 = GameObject.FindWithTag("Player");
        controller = jugador2.GetComponent<CharacterController>();
        camJugador = GameObject.FindWithTag("MainCamera");
        jugadorController = jugador.GetComponent<FirstPersonController>();
        Entrar.enabled = false;
        Salir.enabled = false;

    }

    // Update is called once per frame
    private void Update()
    {
        if (jugador.transform.position != caja.transform.position)
        {
            controller.enabled = controller.enabled;
        }
    }

    /*void OnTriggerEnter (Collision colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            
        }
    }*/
    void OnTriggerStay(Collider colision)//Collider colision)
    {
        // Debug.Log("colision");
        if (camCaja.activeInHierarchy == false)
        {
            Entrar.enabled = true;
        }
        if (colision.gameObject.tag == "Player" && colision.gameObject.tag != "Bala") ;
        {
        if (Input.GetKeyDown(KeyCode.E))
            {
                
                /*posicionjugadorcaja = new Vector3(jugador.transform.position.x, jugador.transform.position.y, jugador.transform.position.z);
                jugador.transform.position = caja.transform.position;
                jugadorcontroller.CollisionFlags = 0;*/
                //controller.enabled = false;
                jugador.SetActive(false);
                camCaja.SetActive(true);
                Entrar.enabled = false;
            }
        }
    }
    void OnTriggerExit()
    {
        Entrar.enabled = false;
    }
}

