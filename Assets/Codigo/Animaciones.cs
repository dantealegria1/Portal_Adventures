using UnityEngine;

public class Animaciones : MonoBehaviour
{
    //Animaciones
    private Animator animator;

    //RigidBody
    private Rigidbody2D rb;

    //Vectores
    private Vector2 lastPosition; 

    /*
    Inicializo mi rigidbody, la ultima posicion de mi agente y mi animator
     */
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lastPosition = rb.position;
    }

    /*
    Obtengo de mi posicion actual mi componente en x y mi componente en y
    Esto se usa para comparar si mi movimiento en x es mayor que en y, para 
    dependiendo del resultado cambiamos nuestra variable movimiento para cambiar 
    mi animacion, dentro de cada uno verifico hacia que sentido es para cambiar 
    entre las diferentes dentro del mismo eje
     */
    void Update()
    {
        Vector2 currentPosition = rb.position;

        float deltaX = currentPosition.x - lastPosition.x;
        float deltaY = currentPosition.y - lastPosition.y;

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
        {
            animator.SetInteger("Movimiento", 1);
            if (deltaX < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetInteger("Movimiento", 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            
            if (deltaY > 0)
            {
                animator.SetInteger("Movimiento", 2);
            }
            else
            {
                animator.SetInteger("Movimiento", 3);
            }
        }

        lastPosition = currentPosition; 
    }
}
