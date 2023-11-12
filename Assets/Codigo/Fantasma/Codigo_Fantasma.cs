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
    public Vector3[] mitadIzquierda;

    void Start()
    {
        crearPosiciones();
        agent = GetComponent<NavMeshAgent>();
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
                agent.SetDestination(destino);   
            }
        }
    }

    Vector3 RandomizarCoordenadas()
    {
        int indice = Random.Range(0, 18);
       
        Vector3 randomCoordinates = mitadDerecha[indice];

        return randomCoordinates;
    }

    void crearPosiciones()
    {
        //la derecha
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
