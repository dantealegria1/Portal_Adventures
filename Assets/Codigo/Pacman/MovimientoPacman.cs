using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPacman : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float Speed;
    private int currentDirection;
    public int TotalMovimientos = 0;
    public Transform PosicionFantasma;
    public Vector2[] MatrizPosiciones;
    private int atorado = 0;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        currentDirection = Random.Range(1, 5);
        MatrizPosiciones = new Vector2[25];
    }

    void Update()
    {
        Libre();
    }

    void Libre()
    {
        Vector2 movement = Vector2.zero;

        switch (currentDirection)
        {
            case 1:
                movement = Vector2.up;
                break;
            case 2:
                movement = Vector2.down;
                break;
            case 3:
                movement = Vector2.left;
                break;
            case 4:
                movement = Vector2.right;
                break;
        }

        if (EsDireccionValida(movement) && TotalMovimientos < 1000)
        {
            _rigidbody.velocity = movement * Speed;
            TotalMovimientos++;

            Trabadillo();

            if (TotalMovimientos > 50)
            {
                Vector2 Posicion1 = MatrizPosiciones[atorado];
                Vector2 Posicion2 = MatrizPosiciones[(atorado - 10 + MatrizPosiciones.Length) % MatrizPosiciones.Length];
                if (Vector2.Distance(Posicion1, Posicion2) < 0.01f)
                {
                    CambiarDireccion();
                    TotalMovimientos = 0;
                }
            }
        }
        else
        {
            CambiarDireccion();
            TotalMovimientos = 0;
        }
    }

    bool EsDireccionValida(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("barrera"));
        return hit.collider == null;
    }

    void Trabadillo()
    {
        Vector2 Posicion = PosicionFantasma.position;
        MatrizPosiciones[atorado] = Posicion;
        atorado = (atorado + 1) % MatrizPosiciones.Length;
    }

    void CambiarDireccion()
    {
        int newDirection = Random.Range(1, 5);
        while (newDirection == currentDirection)
        {
            newDirection = Random.Range(1, 5);
        }
        currentDirection = newDirection;
    }
}
