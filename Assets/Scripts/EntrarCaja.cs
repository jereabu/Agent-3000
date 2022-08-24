﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class EntrarCaja : MonoBehaviour
{
    public GameObject jugador;
    public FirstPersonController jugadorcontroller;
    CharacterController controller;
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
        controller = jugador.GetComponent<CharacterController>();
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
        if (Input.GetKey(KeyCode.I))
        {
            jugador.transform.Translate(Vector3.forward * 0.2f);
        }
        // 1. mover el reigidbody 2. desactivar solo lascolisiones del charactercpntroller 3. ontrigger exit vuelvaelcharacter controller
    }

    void OntriggerEnter (Collision colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            Entrar.enabled = true;
        }
    }
    void OnTriggerStay(Collision colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                /*posicionjugadorcaja = new Vector3(jugador.transform.position.x, jugador.transform.position.y, jugador.transform.position.z);
                jugador.transform.position = caja.transform.position;
                jugadorcontroller.CollisionFlags = 0;*/
                controller.enabled = false;
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

