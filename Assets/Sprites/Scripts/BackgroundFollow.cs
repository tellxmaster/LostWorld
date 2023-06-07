using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform player; // El jugador
    public Vector3 offset; // El desplazamiento del fondo respecto al jugador

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, offset.y, offset.z);
    }
}
