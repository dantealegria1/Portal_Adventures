using UnityEngine;
using UnityEngine.AI;

public class Codigo_Fantasma : MonoBehaviour
{
    public Transform objeto1; // Debes asignar el primer objeto en el Inspector
    public Transform objeto2; // Debes asignar el segundo objeto en el Inspector
    [SerializeField] Transform target; // Declarar la variable target aquí para que sea accesible en el Inspector
    NavMeshAgent agent;
    public Vector3 inicio = new Vector3(0.33f, 2.76f, 0f);
    public Vector3[] mitadDerecha;


    void Start()
    {
        crearPosiciones();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(inicio);
    }

    void Update()
    {
        if (objeto1 != null && objeto2 != null)
        {
            float distancia = Vector3.Distance(objeto1.position, objeto2.position);
            if (distancia < 10.0f)
            {
                InicializarPerseguir();
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
                Vector3 destino = RandomizarCoordenadas();
                Debug.Log(destino);
                agent.SetDestination(destino);   
            }
        }
    }

    Vector3 RandomizarCoordenadas()
    {
        int indice = Random.Range(0, 7);
       
        Vector3 randomCoordinates = mitadDerecha[indice];

        return randomCoordinates;
    }

    void crearPosiciones()
    {
        mitadDerecha = new Vector3[9];
        mitadDerecha[0] = new Vector3(4f, 13f, 0f);
        mitadDerecha[1] = new Vector3(26f, 12f, 0f);
        mitadDerecha[2] = new Vector3(17f, 4f, 0f);
        mitadDerecha[3] = new Vector3(4f, 4f, 0f);
        mitadDerecha[4] = new Vector3(17f, -7f, 0f);
        mitadDerecha[5] = new Vector3(4f, -6f, 0f);
        mitadDerecha[6] = new Vector3(4f, -12f, 0f);
        mitadDerecha[7] = new Vector3(17f, -12f, 0f);
        mitadDerecha[8] = new Vector3(26f, 12f, 0f);
    }
}
