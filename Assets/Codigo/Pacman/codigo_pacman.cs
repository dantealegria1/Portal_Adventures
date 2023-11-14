using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class codigo_pacman : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform target_libre;
    [SerializeField] string tagComida = "comer";
    NavMeshAgent agent;
    public List<Transform> objetivos;  // Cambiado a una lista de objetivos
    public List<Transform> portales;
    public Vector3 borde_izquierdo = new Vector3(-14.62f, -3.89f, 0f);
    public Vector3 borde_derecho = new Vector3(35.99f, -4.04f, 0f);
    public float distanciaEscape = 8f;
    GameObject[] objetosComida;
    public Stack<GameObject> pilaComidas;
    public GameObject comida1;
    public GameObject comida2;
    public GameObject comida3;
    public LayerMask capaBarrera; // Capa que representa las barreras en tu escena
    public float distanciaRaycast = 2f;

    void Start()
    {
        pilaComidas = new Stack<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        objetosComida = GameObject.FindGameObjectsWithTag(tagComida);
        pilaComidas.Push(comida1);
        pilaComidas.Push(comida2);
        pilaComidas.Push(comida3);
        Libre();
    }

    private float tiempoEsperaLibre = 1f; // Ajusta el tiempo de espera según tus necesidades

    void Update()
    {
        CambiarLado();
        float distanciaAlObjetivo = Vector3.Distance(transform.position, GetObjetivoMasCercano(objetivos).position);
        float distanciaAlPortal = Vector3.Distance(transform.position, GetObjetivoMasCercano(portales).position);
        // Usar un umbral combinado para considerar la distancia en todas las direcciones
        float umbralDistancia = 5f;
        if (distanciaAlObjetivo < umbralDistancia)
        {
           if(10 < distanciaAlPortal)
            {
                Escapar();
            }
            else
            {
                agent.SetDestination(GetObjetivoMasCercano(portales).position);
            }
        }
        else
        {
            if (!IsInvoking("Libre"))
            {
                Invoke("Libre", tiempoEsperaLibre);
            }
        }
    }


    void InicializarPerseguir()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (objetivos.Count > 0)
        {
            target = objetivos[0];  // Establece el primer objetivo por defecto
            agent.SetDestination(target.position);
        }
    }

    void Libre()
    {
        if (pilaComidas.Count > 0)
        {
            GameObject primerComida = pilaComidas.Peek();
            agent.SetDestination(primerComida.transform.position);
            if (Vector3.Distance(transform.position, primerComida.transform.position) < 1f)
            {

                MoverHaciaSiguienteComida();
            }
        }
    }

    void MoverHaciaSiguienteComida()
    {

        if (pilaComidas.Count > 0)
        {
            pilaComidas.Pop();
            if (pilaComidas.Count > 0)
            {
                GameObject siguienteComida = pilaComidas.Peek();
                agent.SetDestination(siguienteComida.transform.position);
                Debug.Log("Siguiente comida: " + siguienteComida.name);
            }
        }
    }

    void Escapar()
    {
        if (objetivos.Count > 0)
        {
            Transform objetivoMasCercano = GetObjetivoMasCercano(objetivos);
            Vector3 direccionAlejarse = transform.position - objetivoMasCercano.position;

                Vector3 nuevaPosicion = transform.position + direccionAlejarse.normalized * 10f; // Ajusta el valor 5f según sea necesario
                agent.SetDestination(nuevaPosicion);
        }
    }


    Transform GetObjetivoMasCercano(List<Transform> listaDeObjetivos)
    {
        Transform objetivoMasCercano = null;
        float distanciaMasCercana = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (Transform objetivo in listaDeObjetivos)
        {
            float distancia = Vector3.Distance(posicionActual, objetivo.position);
            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                objetivoMasCercano = objetivo;
            }
        }

        return objetivoMasCercano;
    }

    void CambiarLado()
    {
        Vector3 actual = transform.position;
        Vector3 Portalito = GetObjetivoMasCercano(portales).position;
        float Distancia = Vector3.Distance(actual, Portalito);
        if(Distancia<2f)
        {
            //Vector3 NuevaPosicion = portales[randomInt].position;
            Vector3 NuevaPosicion = new Vector3(0f, 0f, 0f);
            transform.position = NuevaPosicion;
            Debug.Log(transform.position);
        }
    }

}