using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionMultijugador : MonoBehaviourPunCallbacks
{
    private PhotonView playerPhotonView;  // Referencia al PhotonView del jugador
    private GameObject playerObject;      // Referencia al GameObject del jugador
    private float subiendoAnterior;
    public float umbral = 0.01f;          // Umbral para detectar el cambio en Y
    private Collider2D objectCollider;

    [SerializeField]
    private int playerViewID; // Asigna el ViewID del jugador al que queremos seguir

    void Start()
    {
        objectCollider = GetComponent<Collider2D>(); // Obtener el Collider del propio objeto
    }

    void Update()
    {
        // Verifica si el objeto del jugador ya est� asignado
        if (playerPhotonView == null)
        {
            // Buscar el PhotonView usando el ViewID
            playerPhotonView = PhotonView.Find(playerViewID);

            // Aseg�rate de que el PhotonView ha sido encontrado
            if (playerPhotonView != null)
            {
                playerObject = playerPhotonView.gameObject; // Obtener el GameObject del jugador
                subiendoAnterior = playerObject.transform.position.y;
                Debug.Log("Jugador encontrado con PhotonView, posici�n inicial en Y: " + subiendoAnterior);
            }
            return; // Espera hasta que se encuentre el objeto
        }

        // Si el jugador ya ha sido encontrado, proceder con la detecci�n de movimiento
        float subiendoActual = playerObject.transform.position.y;

        // Depuraci�n: Verificar las posiciones actuales y anteriores
        Debug.Log("Posici�n actual del jugador: " + subiendoActual + ", Posici�n anterior: " + subiendoAnterior);

        // Comparamos las posiciones con un umbral para evitar problemas de precisi�n
        if (subiendoActual > subiendoAnterior + umbral)
        {
            if (objectCollider.enabled)
            {
                objectCollider.enabled = false; // Desactiva la colisi�n
                Debug.Log("Est� subiendo, colisi�n desactivada");
            }
        }
        else if (subiendoActual < subiendoAnterior - umbral)
        {
            if (!objectCollider.enabled)
            {
                objectCollider.enabled = true; // Activa la colisi�n
                Debug.Log("Est� bajando, colisi�n activada");
            }
        }

        // Actualizar la posici�n anterior despu�s de la comparaci�n
        subiendoAnterior = subiendoActual;
    }
}