
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 6f;

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

            agent.speed = maxSpeed;
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

        public void StartMoveAction(
            Vector3 destination,
            float speedFraction = 1f)
        {
            if (!CanMove()) return;

            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(
            Vector3 destination,
            float speedFraction = 1f)
        {
            if (!CanMove()) return;

            SetSpeedFraction(speedFraction);
            agent.SetDestination(destination);
        }

        public void SetSpeedFraction(float speedFraction)
        {
            if (agent == null) return;

            agent.speed =
                maxSpeed * Mathf.Clamp01(speedFraction);
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
