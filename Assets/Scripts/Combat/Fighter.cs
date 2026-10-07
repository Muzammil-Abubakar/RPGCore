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

        Transform target;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (GetDistanceToTarget() > weaponRange)
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        private void AttackBehavior()
        {
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            // SetTrigger("attack") starts the attack animation.
            // The attack animation will automatically call the Hit(), its an Animation Event.
            animator.SetTrigger("attack");

            timeSinceLastAttack = 0f;
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;

            Health health = target.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(weaponDamage);
            }
        }

        private float GetDistanceToTarget()
        {
            return Vector3.Distance(
                transform.position,
                target.position
            );
        }
    }
}