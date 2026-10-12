
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
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        Fighter fighter;
        Mover mover;
        GameObject player;
        Health playerHealth;
        Health health;
        ActionScheduler actionScheduler;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        int currentWaypointIndex = 0;
        bool shouldFindNearestWaypoint = true;
        bool isDwelling = false;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            guardPosition = transform.position;

            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        private void Update()
        {
            if (health != null && health.IsDead()) return;

            if (CanAttackPlayer())
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                GuardBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private bool CanAttackPlayer()
        {
            return player != null &&
                   playerHealth != null &&
                   !playerHealth.IsDead() &&
                   DistanceToPlayer() < chaseDistance &&
                   fighter.CanAttack(player);
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
            timeSinceLastSawPlayer = 0f;
            shouldFindNearestWaypoint = true;
            isDwelling = false;
        }

        private void SuspicionBehavior()
        {
            actionScheduler.CancelCurrentAction();
        }

        private void GuardBehavior()
        {
            if (patrolPath == null ||
                patrolPath.GetWaypointCount() == 0)
            {
                mover.StartMoveAction(guardPosition);
                return;
            }

            PatrolBehavior();
        }

        private void PatrolBehavior()
        {
            if (shouldFindNearestWaypoint)
            {
                currentWaypointIndex = FindNearestWaypoint();
                shouldFindNearestWaypoint = false;
                isDwelling = false;
                timeSinceArrivedAtWaypoint = Mathf.Infinity;
            }

            Vector3 waypointPosition =
                patrolPath.GetWaypoint(currentWaypointIndex);

            if (!isDwelling &&
                Vector3.Distance(transform.position,
                    waypointPosition) < waypointTolerance)
            {
                timeSinceArrivedAtWaypoint = 0f;
                isDwelling = true;
                actionScheduler.CancelCurrentAction();
            }

            if (isDwelling)
            {
                if (timeSinceArrivedAtWaypoint < waypointDwellTime)
                {
                    return;
                }

                CycleWaypoint();
                isDwelling = false;

                waypointPosition =
                    patrolPath.GetWaypoint(currentWaypointIndex);
            }

            mover.StartMoveAction(waypointPosition);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex =
                patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private int FindNearestWaypoint()
        {
            int nearestIndex = 0;
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < patrolPath.GetWaypointCount(); i++)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    patrolPath.GetWaypoint(i)
                );

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
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
