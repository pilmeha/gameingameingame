using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

public class Duck : MonoBehaviour, IPointerClickHandler
{
    private static readonly int DamagedHash = Animator.StringToHash("Damaged");

    public DuckStatus Status { get; private set; }

    [SerializeField] private float flightSpeed = 10f;
    [SerializeField] private DuckDiffuculty difficulty = DuckDiffuculty.Low;
    [SerializeField] private float difficultyCompensation = 1.5f;

    public float FlightTime => flightSpeed / (float)difficulty * difficultyCompensation;

    [SerializeField] private SplineContainer spline;

    private Coroutine flightCoroutine;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        Status = DuckStatus.Alive;
        gameObject.SetActive(true);
        flightCoroutine = StartCoroutine(nameof(Fly));
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator Fly()
    {
        float timer = 0f;

        while (timer < FlightTime)
        {
            timer += Time.deltaTime;
            // Debug.Log($"Waiting for {FlightTime} seconds - {FlightTime - timer} to go");
            
            float t = timer / FlightTime;
            Vector3 splinePoint = spline.EvaluatePosition(t);
            Vector3 worldPoint = spline.transform.TransformPoint(splinePoint);
            
            transform.position = worldPoint;

            yield return null;
        }
        Deactivate();
    }

    public void SetPath(SplineContainer path)
    {
        spline = path;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Duck hit!");
        Status = DuckStatus.Damaged;
        if (flightCoroutine != null)
            StopCoroutine(flightCoroutine);
        animator.enabled = true;
        animator.Play(DamagedHash);
    }

    public void SetDifficulty(DuckDiffuculty difficulty)
    {
        this.difficulty = difficulty;
    }
}
