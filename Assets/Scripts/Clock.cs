using UnityEngine;
using UnityEditor;
using System;

public class Clock : MonoBehaviour
{
    [Range(0, 0.2f)]
    public float tickSizeSecOrMin= 0.05f;

    [Range(0, 0.2f)]
    public float tickSizeHour = 0.05f;

    public bool smoothSeconds;

    public bool use24Clock;

    int hoursToClock => use24Clock ? 24 : 12;

    public void OnDrawGizmos()
    {
        Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;
        Handles.DrawWireDisc(default, Vector3.forward, 1);

        for (int i = 0; i < 60; i++)
        {
            Vector2 dir = SecondsOrMinutesToDirection(i);
            DrawTickSecOrMin(dir, tickSizeSecOrMin, 1);
        }

        for (int i = 0; i < hoursToClock; i++)
        {
            Vector2 dir = HourToDirection(i);
            DrawTickSecOrMin(dir, tickSizeHour, 3);
        }

        //Hands
        DateTime time = DateTime.Now;
        float seconds = time.Second;
        if (smoothSeconds)
            seconds += time.Millisecond / 1000f;
        
        ClockHand(SecondsOrMinutesToDirection(seconds), 0.9f, 1, Color.red);
        ClockHand(SecondsOrMinutesToDirection(time.Minute), 0.7f, 4, Color.white);
        ClockHand(HourToDirection(time.Hour), 0.5f, 8, Color.white);
    }

    void DrawTickSecOrMin(Vector2 dir,float length,float thickness)
    {
        Handles.DrawLine(dir, dir * (1 - length), thickness);
    }

    void ClockHand(Vector2 dir ,float length, float thickness ,Color color)
    {
        using (new Handles.DrawingScope(color))
        Handles.DrawLine(default, dir * length, thickness);
    }

    Vector2 SecondsOrMinutesToDirection(float secOrMin)
    {
        return ValueToDirection(secOrMin, 60);
    }

    Vector2 HourToDirection(float hours)
    {
        return ValueToDirection(hours, hoursToClock);
    }
    Vector2 ValueToDirection(float value,float max)
    {
        float t = value / max;
        return FractionToClockDirection(t);
    }
    Vector2 FractionToClockDirection(float t)
    {
        float angleRad = (0.25f -t) * TAU;
        return AngleToDirection(angleRad);
    }

    //Math utility
    const float TAU = 6.28318530718f;
    static Vector2 AngleToDirection(float angleRad)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
