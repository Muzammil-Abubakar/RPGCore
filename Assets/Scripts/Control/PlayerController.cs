
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera mainCamera;
        private Mover mover;
        private Fighter fighter;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
            mainCamera = Camera.main;
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            Debug.Log("Nothing to do.");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            // Check nearest hits first.
            System.Array.Sort(hits, (a, b) =>
                a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target =
                    hit.transform.GetComponent<CombatTarget>();

                // Skip objects without a CombatTarget.
                if (target == null)
                {
                    continue;
                }

                // Check if the target is alive and attackable.
                if (!fighter.CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Mouse.current != null &&
                    Mouse.current.leftButton.wasPressedThisFrame)
                {
                    fighter.Attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (Mouse.current != null &&
                    Mouse.current.leftButton.isPressed)
                {
                    mover.StartMoveAction(hit.point);
                }

                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            return mainCamera.ScreenPointToRay(mousePosition);
        }
    }
}
