using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Guardia : MonoBehaviour {

	[SerializeField] public float Vel = 5;
    [SerializeField]  public float Espera = .3f;
    [SerializeField]  public float VelGiro = 90;
    [SerializeField]  public Transform Camino;
    [SerializeField]  public Light Linterna;
    [SerializeField]  public float DistanciaVista;
    [SerializeField]  public LayerMask VerMask;
    [SerializeField]  float VerAngulo;
    [SerializeField] Transform Jugador;
    [SerializeField] Color LinternaOriginal;
    [SerializeField] public Text Avistado;
    public CharacterController controller;
    public string SandBox;


    void Start() {

        Avistado.enabled = false;
		Jugador = GameObject.FindGameObjectWithTag ("Player").transform;
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
		if (VerJugador ()) {
			Linterna.color = Color.red;
            Avistado.enabled = true;
            Time.timeScale = 0;
            controller.enabled = false;
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SandBox");
                Time.timeScale = 1;
            }
        } else {
			Linterna.color = LinternaOriginal;
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