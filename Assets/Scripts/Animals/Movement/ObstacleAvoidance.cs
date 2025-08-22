using UnityEngine;

public class ObstacleAvoidance
{
    // Casts forward; if we see an obstacle, steer away a bit
    public static Vector3 Steer(Vector3 currentDir, Vector3 position, float castRadius, float lookAhead,
        float turn, LayerMask obstacleMask)
    {
        if (currentDir.sqrMagnitude < 0.0001f) return currentDir;
        currentDir.Normalize();

        Vector3 origin = position + Vector3.up * 0.1f; // tiny lift to avoid ground hits
        if (Physics.SphereCast(origin, castRadius, currentDir, out var hit, lookAhead, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            // Turn away from the surface normal (push sideways)
            Vector3 away = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;
            Vector3 steered = Vector3.Slerp(currentDir, away, turn);
            steered.y = 0f;
            return steered.normalized;
        }
        return currentDir;
    }
}
