using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class codigo_pacman : MonoBehaviour
{
    //Variable Global
    public static bool PowerUP = false;

    //SerializeField
    [SerializeField] Transform target;
    [SerializeField] Transform target_libre;

    //Navmesh
    NavMeshAgent agent;

    //Listas
    public List<Transform> objetivos;  
    public List<Transform> portales;

    //Vectores
    public Vector3 Inicio = new Vector3(0f, 0f, 0f);

    //Float
    public float distanciaEscape = 8f;
    private float tiempoEsperaLibre = 0.5f;
    public float umbralDistancia = 5f;

    //Pilas
    public Stack<GameObject> pilaComidas = new Stack<GameObject>();
    public Stack<GameObject> pilavidas = new Stack<GameObject>();

    //GameObject
    public GameObject comida1;
    public GameObject comida2;
    public GameObject comida3;

    //String
    public string perdiste;
    public string ganaste; 

    //Aqui inicia, se llenan las pilas de vida y de comida asi como encontrar
    //al agente de navmesh
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Comida();
        Libre();
        vidas();
    }

    //Actualiza cada fotograman
    void Update()
    {
        if (pilaComidas.Count > 0 && pilavidas.Count > 0)
        {
            CambiarLado();

            float distanciaAlObjetivo = Vector3.Distance(transform.position, GetObjetivoMasCercano(objetivos).position);
            float distanciaAlPortal = Vector3.Distance(transform.position, GetObjetivoMasCercano(portales).position);
            float distanciaChoque = Vector3.Distance(transform.position, GetObjetivoMasCercano(objetivos).position);
     
            if (distanciaAlObjetivo < umbralDistancia)
            {
                if (10 < distanciaAlPortal)
                {
                    Escapar();

                    if (distanciaChoque < 1.2f)
                    {
                        muerte();
                    }

                }
                else
                {
                    if(distanciaChoque < 1.2f)
                    {
                        muerte();
                    }
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
        else
        {
            if (pilaComidas.Count > 0)
            {
                SceneManager.LoadScene(ganaste);
            }
            else
            {
                SceneManager.LoadScene(perdiste);
            }
        }

    }

    //Estado Perseguir
    void InicializarPerseguir()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (objetivos.Count > 0)
        {
            target = objetivos[0];  
            agent.SetDestination(target.position);
        }
    }

    //Estado Libre
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

    //Para que se mueva a la sigueinte coomida
    void MoverHaciaSiguienteComida()
    {
        if (pilaComidas.Count > 0)
        {
            pilaComidas.Pop();
            if (pilaComidas.Count > 0)
            {
                GameObject siguienteComida = pilaComidas.Peek();
                agent.SetDestination(siguienteComida.transform.position);
            }
        }
    }

    //Estado Escapar
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

    //Obtiene el objjetivo mas cercano
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

    //Estado Teletrannsportar
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

    //Inicializar vidas
    void vidas()
    {
        for(int i=0; i<4; i++)
        {
            GameObject vida = new GameObject("Vida " + (i + 1));
            pilavidas.Push(vida);
        }
    }

    //Inicializar comida
    void Comida()
    {
        pilaComidas.Push(comida1);
        pilaComidas.Push(comida2);
        pilaComidas.Push(comida3);
    }

    //Estado Muerte
    void muerte()
    {
        transform.position = Inicio;
        pilavidas.Pop();
    }

    //Cierra el juego
    void CerrarJuego()
    {
                // Verificar si la aplicación se está ejecutando en el editor de Unity
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Si no está en el editor, cerrar la aplicación
                Application.Quit();
        #endif
    }

}