using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineSmoother : MonoBehaviour
{
    LineRenderer line;
    public Transform[] transformPoints;
    Vector3[] points;
    BazierCurve[] curves;
    [SerializeField]
    float smoothinglength = .5f;
    [SerializeField]
    int smoothingScentions = 10;
    private void Start()
    {
        line = GetComponent<LineRenderer>();

        if(transformPoints.Length > 1){
            curves = new BazierCurve[transformPoints.Length];
            points = new Vector3[transformPoints.Length];
            for (int i = 0; i < transformPoints.Length; i++)
            {
                points[i] = transformPoints[i].position;
            }
        }else{
            curves = new BazierCurve[points.Length];
        }

        for (int i = 0; i < curves.Length; i++)
        {
            curves[i] = new BazierCurve();
        }

        line.positionCount = (curves.Length-1) * smoothingScentions;
    }
    private void Update()
    {
        if(transformPoints.Length > 1){
            for (int i = 0; i < transformPoints.Length; i++)
            {
                points[i] = transformPoints[i].position;
            }
        }

        for (int i = 0; i < curves.Length - 1; i++)
        {
            CalculateSegment(i);
        }

        // set first
        Vector3 nextDirection = (curves[1].EndPostion - curves[1].StartPosition).normalized;
        Vector3 lastDirection = (curves[0].EndPostion - curves[0].StartPosition).normalized;
        curves[0].points[2] = curves[0].points[3] + (nextDirection + lastDirection) * -1 * smoothinglength;

        // draw segment
        Vector3[] allPoints = new Vector3[curves.Length * smoothingScentions];
        int pointsId = 0;
        for (int i = 0; i < curves.Length; i++)
        {
            Vector3[] segments = curves[i].GetSegment(smoothingScentions);
            for (int j = 0; j < segments.Length; j++)
            {
                allPoints[pointsId] = segments[j];
                pointsId++;
            }
        }
        line.SetPositions(allPoints);
    }
    void CalculateSegment(int id)
    {
        // preperation for curving
        Vector3 lastPosition = id == 0 ? points[0] : points[id - 1];
        Vector3 nextPosition = points[id + 1];

        Vector3 lastDirection = (points[id] - lastPosition).normalized;
        Vector3 nextDirection = (nextPosition - points[id]).normalized;

        Vector3 startTangent = (lastDirection + nextDirection) * smoothinglength;
        Vector3 endTangent = (nextDirection + lastDirection) * -1 * smoothinglength;

        curves[id].points[0] = points[id]; // start pos
        curves[id].points[1] = points[id] + startTangent; // start tangen
        curves[id].points[2] = nextPosition + endTangent; // end tangen
        curves[id].points[3] = nextPosition; // end pos
    }
}


