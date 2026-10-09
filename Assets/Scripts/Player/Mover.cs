
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        Animator animator;
        ActionScheduler actionScheduler;
        Health health;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health != null && health.IsDead())
            {
                if (agent.enabled)
                {
                    agent.enabled = false;
                }

                animator.SetFloat("forwardSpeed", 0f);
                return;
            }

            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            if (!CanMove())
            {
                animator.SetFloat("forwardSpeed", 0f);
                return;
            }

            Vector3 velocity = agent.velocity;

            Vector3 localVelocity =
                transform.InverseTransformDirection(velocity);

            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void StartMoveAction(Vector3 destination)
        {
            if (!CanMove()) return;

            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            if (!CanMove()) return;

            agent.SetDestination(destination);
        }

        public void Cancel()
        {
            if (!CanMove()) return;

            agent.ResetPath();
        }

        private bool CanMove()
        {
            if (health != null && health.IsDead())
            {
                return false;
            }

            return agent != null &&
                   agent.enabled &&
                   agent.isOnNavMesh;
        }
    }
}
