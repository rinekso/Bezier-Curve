using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazierCurve : MonoBehaviour
{
    public Vector3[] points;
    public BazierCurve()
    {
        points = new Vector3[4];
    }
    public BazierCurve(Vector3[] points)
    {
        this.points = points;
    }
    public Vector3 StartPosition
    {
        get
        {
            return points[0];
        }
    }
    public Vector3 EndPostion
    {
        get
        {
            return points[3];
        }
    }
    public Vector3 GetSegment(float time){
        time = Mathf.Clamp01(time);
        float timeM = 1-time;
        return (timeM * timeM * timeM * points[0])
            + (3*timeM*timeM*time*points[1])
            + (3*timeM*time*time*points[2])
            + (time*time*time*points[3]);
    }
    public Vector3[] GetSegment(int subdevision){
        Vector3[] segments = new Vector3[subdevision];
        float time;
        for (int i = 0; i < subdevision; i++)
        {
            time = (float)i/subdevision;
            segments[i] = GetSegment(time);
        }
        return segments;
    }
}
