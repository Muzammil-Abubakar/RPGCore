using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;
        Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (target == null) return;

            if (GetDistanceToTarget() > weaponRange)
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Stop();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
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