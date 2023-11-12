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
    public Vector3 borde_izquierdo = new Vector3(-27f, 0.9736717f, 0f);
    public Vector3 borde_derecho = new Vector3(27f, 0.9736717f, 0f);
    public float distanciaEscape = 8f;
    GameObject[] objetosComida;
    public Stack<GameObject> pilaComidas;
    public GameObject comida1;
    public GameObject comida2;


    void Start()
    {
        pilaComidas = new Stack<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        objetosComida = GameObject.FindGameObjectsWithTag(tagComida);
        pilaComidas.Push(comida1);
        pilaComidas.Push(comida2);

    }

    void Update()
    {
        float distanciaAlObjetivo = Vector3.Distance(transform.position, GetObjetivoMasCercano().position);

        if (distanciaAlObjetivo < 10)
        {
      
            Escapar();
        }
        else
        {

            Libre();
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
        float movimiento = 5f;
        if (objetivos.Count > 0)
        {
            Vector3 punto = GetObjetivoMasCercano().position;
            Vector3 Nuevaposicion = new Vector3();
            Vector3 posActual = transform.position;
            if (punto.x > posActual.x)
            {
                Nuevaposicion.x = posActual.x - movimiento;
            }
            else
            {
                Nuevaposicion.x = posActual.x + movimiento; 
            }
            if (punto.y > posActual.y)
            {
                Nuevaposicion.y = posActual.y - movimiento;
            }
            else
            {
                Nuevaposicion.y = posActual.y + movimiento;
            }

            agent.SetDestination(Nuevaposicion);
        }
    }

    Transform GetObjetivoMasCercano()
    {
        Transform objetivoMasCercano = null;
        float distanciaMasCercana = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (Transform objetivo in objetivos)
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

        if (actual.y > 0 && actual.y < 1)
        {
            if (actual.x == 26)
            {
                transform.position = borde_izquierdo;
            }
            else if (actual.x == -26)
            {
                transform.position = borde_derecho;
            }
        }
    }

  
}
