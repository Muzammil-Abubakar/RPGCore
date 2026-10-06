using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        Animator animator;
        ActionScheduler actionScheduler;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            UpdateVelocity();
        }
        private void UpdateVelocity()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity =
                transform.InverseTransformDirection(velocity);

            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void StartMoveAction(Vector3 destination)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public void Cancel()
        {
            agent.ResetPath();
        }

    }
}