using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private float stopDuration = 0.15f;

    private bool isBraking;
    private Vector3 brakingVelocity;
    private float brakingTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckForStop();

        if (isBraking)
        {
            UpdateBraking();
        }

        UpdateVelocity();
    }

    public void MoveTo(Vector3 destination)
    {
        // If we click somewhere while braking,
        // immediately allow movement again.
        isBraking = false;
        brakingVelocity = Vector3.zero;

        agent.SetDestination(destination);
    }

    private void CheckForStop()
    {
        if (Mouse.current == null ||
            !Mouse.current.rightButton.wasPressedThisFrame)
        {
            return;
        }

        // Remember our current movement before cancelling the path.
        brakingVelocity = agent.velocity;

        // Remove the current destination.
        agent.ResetPath();

        brakingTimer = 0f;
        isBraking = true;
    }

    private void UpdateBraking()
    {
        brakingTimer += Time.deltaTime;

        float progress = brakingTimer / stopDuration;

        // Quickly reduce movement from current speed to zero.
        Vector3 currentVelocity =
            Vector3.Lerp(
                brakingVelocity,
                Vector3.zero,
                progress
            );

        agent.Move(currentVelocity * Time.deltaTime);

        if (brakingTimer >= stopDuration)
        {
            isBraking = false;
            brakingVelocity = Vector3.zero;
        }
    }

    private void UpdateVelocity()
    {
        Vector3 velocity = isBraking
            ? Vector3.Lerp(
                brakingVelocity,
                Vector3.zero,
                brakingTimer / stopDuration
            )
            : agent.velocity;

        Vector3 localVelocity =
            transform.InverseTransformDirection(velocity);

        animator.SetFloat(
            "forwardSpeed",
            localVelocity.z
        );
    }
}