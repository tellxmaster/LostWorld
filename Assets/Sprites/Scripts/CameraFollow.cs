using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // El jugador
    public Vector3 offset; // El desplazamiento de la cámara respecto al jugador

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
