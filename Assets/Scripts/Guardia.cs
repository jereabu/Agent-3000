using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class Guardia : MonoBehaviour {
    
	[SerializeField] public float Vel = 5;
    [SerializeField] public float Espera = .3f;
    [SerializeField] public float VelGiro = 90;
    [SerializeField] float tiempoVisto;
    [SerializeField] public float tiempoParaVer = .5f;

    [SerializeField] public Transform Camino;
    [SerializeField] public Light Linterna;
    [SerializeField] float DistanciaVista = 5f;
    [SerializeField] public LayerMask VerMask;
	[SerializeField] float VerAngulo;
    
    [SerializeField] Transform Jugador;
    [SerializeField] Color LinternaOriginal;
    
    [SerializeField] public Text Avistado;
    public CharacterController controller;
	[SerializeField] GameObject player;
    [SerializeField] public GameObject Spawnpoint;

    void Start() {

        Camino = transform.Find("Camino").transform;
        //Linterna = GameObject.Find("Linterna").GetComponent<Light>();
        Avistado = GameObject.FindGameObjectWithTag("Avistado").GetComponent<Text>();
        Avistado.enabled = false;
		Jugador = GameObject.FindGameObjectWithTag ("Player").transform;
		player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<CharacterController>();
        Spawnpoint = GameObject.FindGameObjectWithTag("SpawnPlayer");
        VerAngulo = Linterna.spotAngle;
		LinternaOriginal = Linterna.color;
        

        Vector3[] Puntos = new Vector3[Camino.childCount];
		for (int i = 0; i < Puntos.Length; i++) {
			Puntos [i] = Camino.GetChild (i).position;
			Puntos [i] = new Vector3 (Puntos [i].x, transform.position.y, Puntos [i].z);
		}

		StartCoroutine (SeguirCamino (Puntos));

	}
	void Update() {
		if (VerJugador()) 
		{
            tiempoVisto += Time.deltaTime;
            //Debug.Log(Linterna.color);
        }
		else 
		{
			tiempoVisto -= Time.deltaTime;
		}
        
		tiempoVisto = Mathf.Clamp(tiempoVisto, 0, tiempoParaVer);
        Linterna.color = Color.LerpUnclamped(LinternaOriginal, Color.red, tiempoVisto / tiempoParaVer);
		
        
		if (tiempoVisto >= tiempoParaVer)
        {
                Avistado.enabled = true;
                Time.timeScale = 0;
                //controller.enabled = false;
                if (Input.GetKeyDown(KeyCode.R))
                {
					//player.GetComponent<CharacterController>().enabled = true;
                    player.transform.position = Spawnpoint.transform.position;
                    //player.transform.eulerAngles = new Vector3 (0,0,0);
                    //player.transform.localRotation = new Quaternion.euler(0,0,0);
                    SceneManager.LoadScene("Nivel 1");
					Time.timeScale = 1;       
				}
        }
    }

	bool VerJugador() {
		if (Vector3.Distance(transform.position,Jugador.position) < DistanciaVista) {
			Vector3 DirAlJugador = (Jugador.position - transform.position).normalized;
			float AnguloGuardiayJugador = Vector3.Angle (transform.forward, DirAlJugador);
			if (AnguloGuardiayJugador < VerAngulo / 2f) {
				if (!Physics.Linecast (transform.position, Jugador.position, VerMask)) {
					return true;
				}
			}
		}
		return false;
	}

	IEnumerator SeguirCamino(Vector3[] Puntos) {
		transform.position = Puntos [0];

		int PuntosObjetivoIndex = 1;
		Vector3 PuntosObjetivo = Puntos [PuntosObjetivoIndex];
		transform.LookAt (PuntosObjetivo);

		while (true) {
			transform.position = Vector3.MoveTowards (transform.position, PuntosObjetivo, Vel * Time.deltaTime);
			if (transform.position == PuntosObjetivo) {
				PuntosObjetivoIndex = (PuntosObjetivoIndex + 1) % Puntos.Length;
				PuntosObjetivo = Puntos [PuntosObjetivoIndex];
				yield return new WaitForSeconds (Espera);
				yield return StartCoroutine (Girarse (PuntosObjetivo));
			}
			yield return null;
		}
	}

	IEnumerator Girarse(Vector3 lookTarget) {
		Vector3 dirMirar = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirMirar.z, dirMirar.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, VelGiro * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos() {
		Vector3 posInicio = Camino.GetChild (0).position;
		Vector3 posFinal = posInicio;

		foreach (Transform Punto in Camino) {
			Gizmos.DrawSphere (Punto.position, .3f);
			Gizmos.DrawLine (posFinal, Punto.position);
			posFinal = Punto.position;
		}
		Gizmos.DrawLine (posFinal, posInicio);
	
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.forward * DistanciaVista);
	}
	 void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bala")
		{
			Destroy(gameObject);
			Debug.Log("colision");
		}
    }
}