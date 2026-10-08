
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

            // Stop attacking when the target dies.
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

        public void Attack(CombatTarget combatTarget)
        {
            if (combatTarget == null) return;

            Health newTarget = combatTarget.GetComponent<Health>();

            // Do not attack if there is no Health
            // component or the target is already dead.
            if (newTarget == null || newTarget.IsDead())
            {
                return;
            }

            actionScheduler.StartAction(this);
            target = newTarget;
        }

        public void Cancel()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");

            target = null;
        }

        private void AttackBehavior()
        {
            if (target == null || target.IsDead()) return;

            if (timeSinceLastAttack < timeBetweenAttacks)
            {
                return;
            }

            // Attack animation calls Hit()
            // through an Animation Event.
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");

            timeSinceLastAttack = 0f;
        }

        // Animation Event
        void Hit()
        {
            if (target == null || target.IsDead()) return;

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
