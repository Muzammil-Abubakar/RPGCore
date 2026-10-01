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
        private Fighter fighter;

        private void Awake()
        {
            mainCamera = Camera.main;
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithCombat()) return;

            InteractWithMovement();
        }

        private bool InteractWithCombat()
        {
            Ray ray = GetMouseRay();

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target != null)
                {
                    if (Mouse.current != null &&
                        Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        fighter.Attack(target);
                    }

                    return true;
                }
            }

            return false;
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

            return mainCamera.ScreenPointToRay(mousePosition);
        }
    }
}