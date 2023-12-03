using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Codigo_Fantasma : MonoBehaviour
{
    //TextMeshPro
    public TextMeshProUGUI Estado;

    //Coordenadas de diferntes objetos
    public Transform objeto1;
    public Transform objeto2;

    //Declaramos nuestro target
    [SerializeField] Transform target;

    //Navmesh
    NavMeshAgent agent;

    //Vectores
    public Vector3[] mitadDerecha;

    //String
    public string EstadoActual;

    //Inicializo mi agente
    void Start()
    {
        crearPosiciones();
        agent = GetComponent<NavMeshAgent>();
    }

    /*
    Cada fotograma se verifica la distancia entre mi objeto1 y objeto2 los cuales son el enemigo y su objetivo
    Se verifica su distancia si es menor a en este cvaso 6 entonces va iniciar a perseguirlo
    Si no es asi va seguir en su estado libre

    A diferencia del codigo del astronauta este no tiene condiciones de paro, debido a que no las necesita ya que el
    que establece si se sigue jugando o no es el otro codigo dependiendo del contenido de las pilas
     */
    void Update()
    {
        Estado.text = "Estado del Enemigo: " + EstadoActual;
        float distancia = Vector3.Distance(objeto1.position, objeto2.position);
        if (distancia < 6)
        {
            InicializarPerseguir();
            EstadoActual = "Perseguir";
        }
        else
        {
            Libre();
            EstadoActual = "Libre";
        }

    }
    /*
    Estado perseguir
    Aqui se obtiene la posicion de mi target y se dirige directamente hacia el
     */
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
    /*
    Estado libre
    Aqui va estarse moviendo a lo largo del mapa hacia coordenada preestablecidas, en el momento que llegue a su destino
    buscara otro nuevo

    Esto se hace para que al recorrer diferentes zonas del mapa encuentre a su objetivo
     */
    void Libre()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            if (agent.isStopped || agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 destino = RandomizarCoordenadas();
                agent.SetDestination(destino);
            }
        }
    }

    /*
    Esta funcion es un apoyo para libre, ya que elige alguna coordenada aleatoria para poder moverse
     */
    Vector3 RandomizarCoordenadas()
    {
        int indice = Random.Range(0, 18);

        Vector3 randomCoordinates = mitadDerecha[indice];

        return randomCoordinates;
    }

    /*
    Funcion en la que estan declaradas las posiciones a las que puede ir
     */
    void crearPosiciones()
    {
        mitadDerecha = new Vector3[18];
        mitadDerecha[0] = new Vector3(4f, 13f, 0f);
        mitadDerecha[1] = new Vector3(26f, 12f, 0f);
        mitadDerecha[2] = new Vector3(17f, 4f, 0f);
        mitadDerecha[3] = new Vector3(4f, 4f, 0f);
        mitadDerecha[4] = new Vector3(17f, -7f, 0f);
        mitadDerecha[5] = new Vector3(4f, -6f, 0f);
        mitadDerecha[6] = new Vector3(4f, -12f, 0f);
        mitadDerecha[7] = new Vector3(17f, -12f, 0f);
        mitadDerecha[8] = new Vector3(26f, -12f, 0f);
        mitadDerecha[9] = new Vector3(-4.52f, 12.77f, 0f);
        mitadDerecha[10] = new Vector3(-25.96f, 13.01f, 0f);
        mitadDerecha[11] = new Vector3(-17.06f, 9.91f, 0f);
        mitadDerecha[12] = new Vector3(-5.99f, 4.1f, 0f);
        mitadDerecha[13] = new Vector3(-16.85f, -6.53f, 0f);
        mitadDerecha[14] = new Vector3(-4.07f, -5.99f, 0f);
        mitadDerecha[15] = new Vector3(-4.62f, -12.41f, 0f);
        mitadDerecha[16] = new Vector3(-17.17f, -13.11f, 0f);
        mitadDerecha[17] = new Vector3(-25.96f, -13.01f, 0f);

    }
}