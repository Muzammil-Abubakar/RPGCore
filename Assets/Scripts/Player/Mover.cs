using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mainCamera;
    private Animator animator;

    [SerializeField] private float stopDuration = 0.15f;

    private bool isBraking;
    private Vector3 brakingVelocity;
    private float brakingTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckForStop();

        if (isBraking)
        {
            UpdateBraking();
        }
        else
        {
            MoveToCursor();
        }

        UpdateVelocity();
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

        // Quickly reduce the movement from current speed to zero.
        Vector3 currentVelocity =
            Vector3.Lerp(brakingVelocity, Vector3.zero, progress);

        agent.Move(currentVelocity * Time.deltaTime);

        if (brakingTimer >= stopDuration)
        {
            isBraking = false;
            brakingVelocity = Vector3.zero;
        }
    }

    private void MoveToCursor()
    {
        if (Mouse.current == null ||
            !Mouse.current.leftButton.isPressed)
        {
            return;
        }

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            mainCamera.ScreenPointToRay(mousePosition);

        Debug.DrawRay(
            ray.origin,
            ray.direction * 1000f,
            Color.red
        );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.SetDestination(hit.point);
        }
    }

    private void UpdateVelocity()
    {
        Vector3 velocity = isBraking
            ? Vector3.Lerp(
                brakingVelocity,
                Vector3.zero,
                brakingTimer / stopDuration)
            : agent.velocity;

        Vector3 localVelocity =
            transform.InverseTransformDirection(velocity);

        animator.SetFloat(
            "forwardSpeed",
            localVelocity.z
        );
    }
}