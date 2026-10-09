
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 20f;

        Health target;
        Health health;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // Dead characters cannot attack.
            if (health != null && health.IsDead())
            {
                if (target != null)
                {
                    Cancel();
                }

                return;
            }

            if (target == null) return;

            // Stop attacking if the target dies.
            if (target.IsDead())
            {
                Cancel();
                return;
            }

            if (GetDistanceToTarget() > weaponRange)
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            if (health != null && health.IsDead())
            {
                return false;
            }

            Health targetToTest =
                combatTarget.GetComponent<Health>();

            return targetToTest != null &&
                   !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            if (!CanAttack(combatTarget)) return;

            actionScheduler.StartAction(this);

            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");

            target = null;

            mover.Cancel();
        }

        private void AttackBehavior()
        {
            if (target == null || target.IsDead()) return;

            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;

            // Avoid LookAt on an identical position.
            if ((targetPosition - transform.position).sqrMagnitude > 0.001f)
            {
                transform.LookAt(targetPosition);
            }

            if (timeSinceLastAttack < timeBetweenAttacks)
            {
                return;
            }

            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");

            timeSinceLastAttack = 0f;
        }

        // Animation Event
        void Hit()
        {
            if (target == null || target.IsDead()) return;

            if (health != null && health.IsDead()) return;

            target.TakeDamage(weaponDamage);
        }

        private float GetDistanceToTarget()
        {
            return Vector3.Distance(
                transform.position,
                target.transform.position
            );
        }
    }
}
