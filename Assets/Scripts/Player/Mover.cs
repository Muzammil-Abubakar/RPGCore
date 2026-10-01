using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator animator;
        private Fighter fighter;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateVelocity();
        }

        public void StartMoveAction(Vector3 destination)
        {
            fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public void Stop()
        {
            agent.ResetPath();
        }

        private void UpdateVelocity()
        {
            Vector3 velocity = agent.velocity;

            Vector3 localVelocity =
                transform.InverseTransformDirection(velocity);

            animator.SetFloat(
                "forwardSpeed",
                localVelocity.z
            );
        }
    }
}