using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class codigo_pacman : MonoBehaviour
{

    public Transform objeto1; // Debes asignar el primer objeto en el Inspector
    public Transform objeto2; // Debes asignar el segundo objeto en el Inspector
    [SerializeField] Transform target; // Declarar la variable target aquí para que sea accesible en el Inspector
    [SerializeField] Transform target_libre;
    [SerializeField] string tagComida = "comer";
    NavMeshAgent agent;
    public Vector3 inicio = new Vector3(0.33f, 2.76f, 0f);
    public Vector3[] mitadDerecha;
    public Vector3[] mitadIzquierda;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (objeto1 != null && objeto2 != null)
        {
            float distancia = Vector3.Distance(objeto1.position, objeto2.position);
            if (distancia < 10.0f)
            {
                //InicializarPerseguir();
                Debug.Log("hola");
            }
            else
            {
                Libre();
            }
        }
        else
        {
            Debug.LogWarning("Make sure to assign the objects in the Inspector.");
        }
    }

    public void InicializarPerseguir()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }
    }

    void Libre()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            if (agent.isStopped || agent.remainingDistance <= agent.stoppingDistance)
            {
                BuscarComida();
            }
        }
    }
    void BuscarComida()
    {
       
        GameObject[] objetosComida = GameObject.FindGameObjectsWithTag(tagComida);
        if (objetosComida.Length > 0)
        {
            GameObject comidaMasCercana = EncontrarComidaMasCercana(objetosComida);
            if (comidaMasCercana != null)
            {
                target_libre = comidaMasCercana.transform;
                MoverHaciaObjetivo();
            }
        }
    }

    GameObject EncontrarComidaMasCercana(GameObject[] objetosComida)
    {
        GameObject comidaMasCercana = null;
        float distanciaMasCercana = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (GameObject comida in objetosComida)
        {
            float distancia = Vector3.Distance(posicionActual, comida.transform.position);
            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                comidaMasCercana = comida;
            }
        }

        return comidaMasCercana;
    }

    void MoverHaciaObjetivo()
    {
        agent.SetDestination(target_libre.position);
    }




}
