using UnityEngine;

public class MovimientoMuñeco : MonoBehaviour
{
    public float velocidad = 5.0f; // La velocidad de movimiento

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el Rigidbody del GameObject
    }

    void FixedUpdate()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal"); // Obtiene el input horizontal (izquierda/derecha)
        float movimientoVertical = Input.GetAxis("Vertical"); // Obtiene el input vertical (adelante/atrás)

        Vector3 movimiento = new Vector3(movimientoHorizontal, 0.0f, movimientoVertical); // Crea un vector con los inputs

        rb.AddForce(movimiento * velocidad); // Aplica una fuerza al Rigidbody basada en el vector de movimiento
    }
}
