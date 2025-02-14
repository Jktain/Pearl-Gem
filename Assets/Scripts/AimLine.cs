using UnityEngine;
using System.Collections.Generic;

public class AimLine : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private int linePoints = 30;
    [SerializeField] private float timeStep = 0.05f;

    public void ShowTrajectory(Vector3 startPosition, Vector3 velocity)
    {
        lineRenderer.positionCount = linePoints;
        List<Vector3> points = new List<Vector3>();

        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = velocity;

        for (int i = 0; i < linePoints; i++)
        {
            points.Add(currentPosition);
            RaycastHit hit;
            if (Physics.Raycast(currentPosition, currentVelocity.normalized, out hit, currentVelocity.magnitude * timeStep, collisionMask))
            {
                points.Add(hit.point);
                break;
            }

            currentPosition += currentVelocity * timeStep;
            currentVelocity += Physics.gravity * timeStep;
        }

        lineRenderer.SetPositions(points.ToArray());
    }

    public void HideTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}
