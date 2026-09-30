using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private Mover mover;

    private void Awake()
    {
        mainCamera = Camera.main;
        mover = GetComponent<Mover>();
    }

    private void Update()
    {
        MoveToCursor();
    }

    private void MoveToCursor()
    {
        if (Mouse.current == null ||
            !Mouse.current.leftButton.isPressed)
        {
            return;
        }

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            mainCamera.ScreenPointToRay(mousePosition);

        Debug.DrawRay(
            ray.origin,
            ray.direction * 1000f,
            Color.red
        );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mover.MoveTo(hit.point);
        }
    }
}