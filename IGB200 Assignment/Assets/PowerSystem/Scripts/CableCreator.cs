using UnityEngine;

public class CableCreator : MonoBehaviour
{
    public GameObject segmentPrefab;  // The prefab for the cable segment
    public GameObject startPoint;     // The first object
    public GameObject endPoint;       // The second object
    public int numberOfSegments = 10; // Number of segments to create
    public float segmentLength = 1.0f; // Length of each segment
    public float cableSpring = 10.0f; // Spring setting for the joint
    public float cableDamper = 1.0f;  // Damper setting for the joint

    void Start()
    {
        // The starting position for the first segment
        Vector3 currentPosition = startPoint.transform.position;

        // The direction from startPoint to endPoint
        Vector3 direction = (endPoint.transform.position - startPoint.transform.position).normalized;

        // Reference to the previous segment
        GameObject previousSegment = startPoint;

        for (int i = 0; i < numberOfSegments; i++)
        {
            // Create a new segment at the current position
            GameObject segment = Instantiate(segmentPrefab, currentPosition, Quaternion.identity, this.transform);

            // Get the SpringJoint component and set its properties
            SpringJoint joint = segment.GetComponent<SpringJoint>();
            joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
            joint.spring = cableSpring;
            joint.damper = cableDamper;

            // Move the currentPosition along the direction by segmentLength
            currentPosition += direction * segmentLength;

            // Update previousSegment to the segment we just created
            previousSegment = segment;
        }

        // Connect the last segment to the endPoint
        SpringJoint endJoint = previousSegment.GetComponent<SpringJoint>();
        endJoint.connectedBody = endPoint.GetComponent<Rigidbody>();
    }
}
