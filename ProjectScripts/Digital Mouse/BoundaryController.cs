using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    public Transform boundaryObject; // Boundary object (should contain a BoxCollider2D)
    private Vector2 boundaryMin; // Minimum boundary
    private Vector2 boundaryMax; // Maximum boundary

    void Start()
    {
        // Get the Collider2D from the boundary object
        Collider2D boundaryCollider = boundaryObject.GetComponent<Collider2D>();

        if (boundaryCollider != null)
        {
            // Get the bounds of the Collider
            boundaryMin = boundaryCollider.bounds.min;
            boundaryMax = boundaryCollider.bounds.max;
        }
        else
        {
            Debug.LogError("Please add a Collider2D to the boundary object!");
        }
    }

    void Update()
    {
        Vector3 position = transform.position;

        // Clamp the object's position within the boundary
        position.x = Mathf.Clamp(position.x, boundaryMin.x, boundaryMax.x);
        position.y = Mathf.Clamp(position.y, boundaryMin.y, boundaryMax.y);

        // Apply the new position
        transform.position = position;
    }
}
