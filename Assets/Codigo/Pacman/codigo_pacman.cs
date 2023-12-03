using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class codigo_pacman : MonoBehaviour
{
    //TextMeshPro
    public TextMeshProUGUI textoMeshPro;
    public TextMeshProUGUI Estado;

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
    public GameObject comida4;
    public GameObject comida5;

    //String
    public string perdiste;
    public string ganaste;
    public string EstadoActual;

    //Funcion inicial para llenar las pilas de comida, asi como tambien mi pila de vidas. 
    //Tambien inicializa a mi agente y lo pone en el estado Libre
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Comida();
        Libre();
        vidas();
    }

    /*
    Cada fotograma se verifica antes que nada si las pilas de comida y de vidas siguen con elementos
    Si su cantidad de elementos es mayor que zero. Calculo la distancia entre los enemigos y mi agente,
    entre mi agente y los diferentes portales
    Si no lo es entonces el juego acaba. Dependiendo de si mi pila de comida esta vacia o la pila de vidas
    entonces habra ganado o perdido
    
    Si mi distancia es menor a nuestro umbral de distancia, cambia a estado escapando, a menos de que tenga
    un portal cerca, si es asi buscara ir al portal
    Si existe contacto, que en este caso lo tomamos como si mi distancia de choque es muy pequena entonces 
    cambiara a muerte
    
    Si es que mi distancia a los enemigos es mayor que mi umbral de distancia entonces estara en estado libre
    */
    void Update()
    {
        textoMeshPro.text = "Vidas Restantes: " + pilavidas.Count.ToString() + 
        "\nComida Faltante: " + pilaComidas.Count.ToString();
        Estado.text = "Estado del Agente: " + EstadoActual;
        Debug.Log(pilaComidas.Count);
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
                    EstadoActual = "Escapar";

                    if (distanciaChoque < 1.2f)
                    {
                        muerte();
                        EstadoActual = "Muerto";
                    }

                }
                else
                {
                    if (distanciaChoque < 1.2f)
                    {
                        muerte();
                        EstadoActual = "Muerto";
                    }
                    EstadoActual = "Perseguir Portal";
                    agent.SetDestination(GetObjetivoMasCercano(portales).position);
                }
            }
            else
            {
                EstadoActual = "Libre";
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
                SceneManager.LoadScene(perdiste);
            }
            else
            {
                SceneManager.LoadScene(ganaste);
            }
        }

    }


    /*
    Estado libre
    Aqui el agente esta buscando la comida, siempre se dirigira hacia la comdida que este en el tope de la pila
    Se agrego una verificacion sobre el contenido de la pila para evitar fallas para el compilador
     */
    void Libre()
    {
        if (pilaComidas.Count > 0)
        {
            GameObject primerComida = pilaComidas.Peek();
            agent.SetDestination(primerComida.transform.position);
            if (Vector3.Distance(transform.position, primerComida.transform.position) < 1f)
            {
                Destroy(primerComida);
                MoverHaciaSiguienteComida();
            }
        }
    }

    /*
    Esta es una funcion que nos ayuda para el correcto funcionamiento de el estado libre, nos ayuda a movernos hacia
    la siguiente comida. esto lo hace sacando de la pila al elemento en el tope de esta, y moviendose hacia la siguiente comida
     */
    void MoverHaciaSiguienteComida()
    {
        if (pilaComidas.Count > 0)
        {
            pilaComidas.Pop();
            GameObject siguienteComida = pilaComidas.Peek();
            agent.SetDestination(siguienteComida.transform.position);
        }
    }

    /*
    Estado escapar
    Despues de obtener la posicion del enemigo mas cercano, se establece una nueva direccion a la que se intentaria alejar de este
     */
    void Escapar()
    {
        Transform objetivoMasCercano = GetObjetivoMasCercano(objetivos);
        Vector3 direccionAlejarse = transform.position - objetivoMasCercano.position;

        Vector3 nuevaPosicion = transform.position + direccionAlejarse.normalized * 10f;
        agent.SetDestination(nuevaPosicion);
    }

    /*
    Funcion base para el funcionamiento de todo el programa ya que es la misma que detecta la distancia entre
    los diferentes elementos del juego y nuestro agente.

    Lo que hace es que recibe una lista de objetivos y de esta ubica cual de estos es el que tengo mas cercano
     */
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

    /*
    Estado teletransportar
    Al momento de entrar en contacto con un portal este lo hara regresar al inicio que es el vector 0,0,0
     */
    void CambiarLado()
    {
        Vector3 actual = transform.position;
        Vector3 Portalito = GetObjetivoMasCercano(portales).position;
        float Distancia = Vector3.Distance(actual, Portalito);
        if (Distancia < 2f)
        {
            Vector3 NuevaPosicion = new Vector3(0f, 0f, 0f);
            transform.position = NuevaPosicion;
        }
    }

    /*
    Funcion que inicializa mi pila de vidas
     */
    void vidas()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject vida = new GameObject("Vida " + (i + 1));
            pilavidas.Push(vida);
        }
    }

    /*
    Funcion que inicializa mi pila de comida
     */
    void Comida()
    {
        pilaComidas.Push(comida1);
        pilaComidas.Push(comida2);
        pilaComidas.Push(comida3);
        pilaComidas.Push(comida4);
        pilaComidas.Push(comida5);
    }

    /*
    Estado Muerte
    Regresa al agente al inicio y quita el elemento en el tope de la pila
     */
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