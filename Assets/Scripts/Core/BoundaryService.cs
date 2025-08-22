using UnityEngine;

public class BoundaryService
{
    private Camera cam;
    public BoundaryService()
    {
        cam = Camera.main;
    }

    public bool IsOutside(Transform t)
    {
        if (cam == null) cam = Camera.main;
        Vector3 v = cam.WorldToViewportPoint(t.position);
        return v.x < 0f || v.x > 1f || v.y < 0f || v.y > 1f;
    }

    public Vector3 ReflectTowardsCenter(Transform t, Vector3 currentDir)
    {
        if (cam == null) cam = Camera.main;
        float planeDist = Mathf.Abs(cam.transform.position.y - t.position.y);
        Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, planeDist));
        Vector3 toCenter = (center - t.position).normalized;
        return Vector3.Slerp(currentDir, toCenter, 0.8f).normalized;
    }
}
