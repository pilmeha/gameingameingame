using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class Duck : MonoBehaviour
{

    public DuckStatus Status { get; private set; }

    [SerializeField] private float flightSpeed = 10f;
    [SerializeField] private float difficulty = 1f;
    [SerializeField] private float difficultyCompensation = 1.5f;

    public float FlightTime => flightSpeed / difficulty * difficultyCompensation;

    [SerializeField] private SplineContainer spline;

    public void Dissolve()
    {
        Status = DuckStatus.Damaged;
    }

    public void Activate()
    {
        Status = DuckStatus.Alive;
        gameObject.SetActive(true);
        // flightCoroutine = new(FlightTime);
        StartCoroutine(nameof(Fly));
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator Fly()
    {
        float timer = 0f;
        Debug.Log($"New duck started to fly");

        while (timer < FlightTime)
        {
            timer += Time.deltaTime;
            // Debug.Log($"Waiting for {FlightTime} seconds - {FlightTime - timer} to go");
            
            float t = timer / FlightTime;
            Vector3 splinePoint = spline.EvaluatePosition(t);
            Vector3 worldPoint = spline.transform.TransformPoint(splinePoint);
            
            transform.localPosition = worldPoint;

            yield return null;
        }
        Deactivate();
    }

    public void SetPath(SplineContainer path)
    {
        spline = path;
    }

    public enum DuckStatus
    {
        Alive,
        Damaged
    }
}
