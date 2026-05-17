using UnityEngine;
using UnityEngine.EventSystems;

public class Dog : MonoBehaviour, IPointerClickHandler
{
    private static readonly int DamagedHash = Animator.StringToHash("Damaged");
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    [SerializeField] private float speed = 5f;

    [SerializeField] private float waveFrequency = 5f;
    [SerializeField] private float waveAmplitude = 2f;

    private Direction direction;
    private Transform currentTarget;
    private Animator animator;
    [SerializeField] private AudioSource hurtSFX;
    private bool canWalk = true;

    public DuckStatus Status { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        ChangeDirection(Direction.Right);
    }
    void Update()
    {
        Move();
        ChangeDirection(direction);
    }

    private void Move()
    {
        if (Vector3.Distance(currentTarget.position, transform.position) < 0.1f)
        {
            Direction dir = Direction.Left;
            if (direction == dir)
                dir = Direction.Right;
            ChangeDirection(dir);
        }
        if (canWalk)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            float sineOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

            transform.position += new Vector3(0f, sineOffset, 0f);
        }
    }

    void ChangeDirection(Direction dir)
    {
        if (direction == dir)
            return;

        direction = dir;
        switch (direction)
        {
            case Direction.Left:
                currentTarget = leftBorder;
                break;
            case Direction.Right:
                currentTarget = rightBorder;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Status == DuckStatus.Damaged || !DuckGameManager.Instance.HasAmmo)
            return;

        Debug.Log($"Dog hit!");
        Hit();
    }

    private void Hit()
    {
        Status = DuckStatus.Damaged;

        animator.enabled = true;
        animator.Play(DamagedHash);

        hurtSFX.Play();
        canWalk = false;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private enum Direction
    {
        Left,
        Right
    }
}
