
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;

        public int GetWaypointCount()
        {
            return transform.childCount;
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }

        public int GetNextIndex(int currentIndex)
        {
            if (GetWaypointCount() == 0) return 0;

            return (currentIndex + 1) % GetWaypointCount();
        }

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
            Vector3 waypointPosition = GetWaypoint(index);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(
                waypointPosition,
                waypointGizmoRadius
            );
        }

        private void DrawConnection(int index)
        {
            Vector3 currentPosition = GetWaypoint(index);
            int nextIndex = GetNextIndex(index);
            Vector3 nextPosition = GetWaypoint(nextIndex);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(
                currentPosition,
                nextPosition
            );
        }
    }
}
