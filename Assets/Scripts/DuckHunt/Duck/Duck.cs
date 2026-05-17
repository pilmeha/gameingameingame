using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

public class Duck : MonoBehaviour, IPointerClickHandler
{
    private static readonly int DamagedHash = Animator.StringToHash("Damaged");
    private static readonly int xPosHash = Animator.StringToHash("xPosition");
    private static readonly int yPosHash = Animator.StringToHash("yPosition");

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
    public int index = 0;

    public float FlightTime => flightSpeed / (float)difficulty * difficultyCompensation;
    public float Score => defaultScore *(float)difficulty;

    public UnityEvent<int> OnSurvived = new();
    public UnityEvent OnDeactivate = new();

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void Activate()
    {
        Status = DuckStatus.Alive;
        gameObject.SetActive(true);
        flightCoroutine = StartCoroutine(nameof(Fly));
    }

    public void Deactivate()
    {
        OnSurvived.RemoveAllListeners();
        gameObject.SetActive(false);
        OnDeactivate.Invoke();
    }

    private IEnumerator Fly()
    {
        float timer = 0f;

        while (timer < FlightTime)
        {
            timer += Time.deltaTime;

            float t = timer / FlightTime;
            Vector3 splinePoint = spline.EvaluatePosition(t);
            Vector3 worldPoint = spline.transform.TransformPoint(splinePoint);
            Vector3 direction = (worldPoint-transform.position).normalized;
            animator.SetFloat(xPosHash, direction.x);
            animator.SetFloat(yPosHash, direction.y);
            transform.position = worldPoint;

            yield return null;
        }
        OnSurvived.Invoke(index);
        Deactivate();
    }

    public void SetPath(SplineContainer path)
    {
        spline = path;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Status == DuckStatus.Damaged || !DuckGameManager.Instance.HasAmmo)
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
