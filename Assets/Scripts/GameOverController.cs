using UnityEngine;

public class GameOverController : MonoBehaviour
{
    void Start()
    {
        // Desbloquear y mostrar el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
