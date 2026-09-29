using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mainCamera;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        MoveToCursor();
        UpdateVelocity();
    }

    private void MoveToCursor()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        Debug.DrawRay(
            ray.origin,
            ray.direction * 1000f,
            Color.red,
            2f
        );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.SetDestination(hit.point);
        }
    }

    private void UpdateVelocity()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        animator.SetFloat("forwardSpeed", localVelocity.z);
    }
}