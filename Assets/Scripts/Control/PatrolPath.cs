
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DrawWaypoint(i);
                DrawConnection(i);
            }
        }

        private void DrawWaypoint(int index)
        {
            Vector3 waypointPosition = GetWaypointPosition(index);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(
                waypointPosition,
                waypointGizmoRadius
            );
        }

        private void DrawConnection(int index)
        {
            Vector3 currentPosition = GetWaypointPosition(index);

            int nextIndex = (index + 1) % transform.childCount;
            Vector3 nextPosition = GetWaypointPosition(nextIndex);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(
                currentPosition,
                nextPosition
            );
        }

        private Vector3 GetWaypointPosition(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}
