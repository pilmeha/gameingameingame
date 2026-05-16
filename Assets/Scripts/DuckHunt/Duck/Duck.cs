using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

public class Duck : MonoBehaviour, IPointerClickHandler
{
    private static readonly int DamagedHash = Animator.StringToHash("Damaged");

    [Header("Parameters")]
    [SerializeField] [EnumButtons] private DuckDiffuculty difficulty = DuckDiffuculty.Low;
    [SerializeField] private float defaultScore = 100f;
    [SerializeField] private float flightSpeed = 10f;
    [SerializeField] private float difficultyCompensation = 1.2f;

    [Header("Sound")]
    [SerializeField] private AudioSource hurtSFX;
    [SerializeField] private AudioSource fallSFX;
    private SplineContainer spline;
    private Coroutine flightCoroutine;
    private Animator animator;

    public DuckStatus Status { get; private set; }

    public float FlightTime => flightSpeed / (float)difficulty * difficultyCompensation;
    public float Score => defaultScore *(float)difficulty;

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
        if (Status == DuckStatus.Damaged)  
            return;

        Debug.Log($"Duck hit!");
        Hit();
    }

    private void Hit()
    {
        Status = DuckStatus.Damaged;
        
        if (flightCoroutine != null)
            StopCoroutine(flightCoroutine);

        animator.enabled = true;
        animator.Play(DamagedHash);

        hurtSFX.Play();
        DuckGameManager.Instance.AddScore(Score);
    }

    public void SetDifficulty(DuckDiffuculty difficulty)
    {
        this.difficulty = difficulty;
    }
}
