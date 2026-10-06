using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;

        Transform target;
        Mover mover;
        ActionScheduler actionScheduler;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
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

            GetComponent<Animator>().SetTrigger("attack");
            timeSinceLastAttack = 0f;
        }

        private float GetDistanceToTarget()
        {
            return Vector3.Distance(
                transform.position,
                target.position
            );
        }

        // Animation Event
        void Hit()
        {
        }
    }
}