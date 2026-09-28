using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            Debug.DrawRay(
                ray.origin,
                ray.direction * 1000f,
                Color.red,
                2f
            );
        }
    }
}