
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        Fighter fighter;
        Mover mover;
        GameObject player;
        Health playerHealth;
        ActionScheduler actionScheduler;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            // Save the original guard position.
            guardPosition = transform.position;

            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        private void Update()
        {
            // Do nothing if this guard is dead.
            Health health = GetComponent<Health>();

            if (health != null && health.IsDead())
            {
                return;
            }

            // Attack only if the player is alive and in range.
            if (player != null &&
                playerHealth != null &&
                !playerHealth.IsDead() &&
                DistanceToPlayer() < chaseDistance &&
                fighter.CanAttack(player))
            {
                AttackBehavior();
                timeSinceLastSawPlayer = 0f;
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                GuardBehavior();
            }

            // Track how long it has been since seeing the player.
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private void SuspicionBehavior()
        {
            // Cancel the current action and remain idle.
            actionScheduler.CancelCurrentAction();
        }

        private void GuardBehavior()
        {
            // Return to the original guard position.
            mover.StartMoveAction(guardPosition);
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(
                player.transform.position,
                transform.position
            );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                transform.position,
                chaseDistance
            );
        }
    }
}
