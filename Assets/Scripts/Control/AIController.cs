
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter fighter;
        Mover mover;
        GameObject player;
        Health playerHealth;

        bool isChasing = false;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        private void Update()
        {
            if (player == null || playerHealth == null)
            {
                StopChasing();
                return;
            }

            // Stop attacking if the player dies.
            if (playerHealth.IsDead())
            {
                StopChasing();
                return;
            }

            // Chase and attack when player is in range.
            if (DistanceToPlayer() < chaseDistance &&
                fighter.CanAttack(player))
            {
                if (!isChasing)
                {
                    fighter.Attack(player);
                    isChasing = true;
                }
            }
            else
            {
                StopChasing();
            }
        }

        private void StopChasing()
        {
            if (!isChasing) return;

            fighter.Cancel();
            mover.Cancel();

            isChasing = false;
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
