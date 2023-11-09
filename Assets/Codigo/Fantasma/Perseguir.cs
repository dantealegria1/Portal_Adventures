using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Perseguir : MonoBehaviour
{
    [SerializeField] Transform target; // Declarar la variable target aquí para que sea accesible en el Inspector
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        InicializarPerseguir();
    }

    // Update is called once per frame
    void Update()
    {
        // Puedes realizar otras operaciones de actualización aquí si es necesario
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
}
