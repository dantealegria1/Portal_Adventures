using UnityEngine;
using UnityEngine.AI;

public class Codigo_Fantasma : MonoBehaviour
{
    public Transform objeto1; // Debes asignar el primer objeto en el Inspector
    public Transform objeto2; // Debes asignar el segundo objeto en el Inspector
    [SerializeField] Transform target; // Declarar la variable target aquí para que sea accesible en el Inspector
    NavMeshAgent agent;

    public float minX = 3f; // Valor mínimo de X
    public float maxX = 25f; // Valor máximo de X
    public float minY = 3f; // Valor mínimo de Y
    public float maxY = 13f; // Valor máximo de Y
    public int TotalMovimientos = 0;
    public int MaxMovimientosAntesCambio = 10; // Número máximo de movimientos antes de cambiar la posición
    public Vector3 inicio = new Vector3(0.33f, 2.76f, 0f);


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(inicio);
    }

    void Update()
    {
        if (objeto1 != null && objeto2 != null)
        {
            float distancia = Vector3.Distance(objeto1.position, objeto2.position);
            if (distancia < 5.0f)
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
            if (TotalMovimientos > 100)
            {
                Vector3 destino = RandomizarCoordenadas();
                Debug.Log(destino);
                agent.SetDestination(destino);
                TotalMovimientos = 0;
            }
            else
            {
                TotalMovimientos++;
            }
        }
    }

    Vector3 RandomizarCoordenadas()
    {
        float randomX = Random.Range(minX, maxX);
        if (randomX < 0)
            randomX = randomX * -1;
        float randomY = Random.Range(minY, maxY);
        if(randomY < 0)
            randomY = randomY * -1;
        // Crear un vector para almacenar las coordenadas aleatorias
        Vector3 randomCoordinates = new Vector3(randomX, 0f, randomY);

        return randomCoordinates;
    }
}
