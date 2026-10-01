using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
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
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            Ray ray = GetMouseRay();

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target != null)
                {
                    Debug.Log("Combat Target Found");
                }
            }
        }

        private void InteractWithMovement()
        {
            if (Mouse.current == null ||
                !Mouse.current.leftButton.isPressed)
            {
                return;
            }

            Ray ray = GetMouseRay();

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                mover.MoveTo(hit.point);
            }
        }

        private Ray GetMouseRay()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            return ray;
        }
    }
}