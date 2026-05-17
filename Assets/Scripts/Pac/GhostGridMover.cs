using UnityEngine;

public class GhostGridMover : MonoBehaviour
{
    public float moveSpeed = 4f;
    public bool isVulnerable = false;
    public Sprite normalSprite;
    public Sprite vulnerableSprite;

    private Vector2 targetPos;
    private Vector2 currentDir;
    private Vector2 intendedDir;
    private Transform player;
    private Vector2 startPos;          // 记录自己的出生点
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    // 用于标记这是第几个鬼（由管理器赋值）
    [HideInInspector] public int ghostIndex = -1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
            if (normalSprite == null)
                normalSprite = originalSprite;
        }

        AlignToGrid();
        targetPos = transform.position;

        currentDir = FindAnyValidDirection();
        intendedDir = currentDir;
        if (currentDir != Vector2.zero)
        {
            targetPos = (Vector2)transform.position + currentDir;
            isMoving = true;
        }
    }

    void AlignToGrid()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            transform.position = targetPos;
            isMoving = false;

            intendedDir = DecideDirection();

            if (IsValidDirection(intendedDir) && !IsCellOccupiedByGhost((Vector2)transform.position + intendedDir))
                currentDir = intendedDir;
            else
                currentDir = FindAvailableDirectionWithGhostAvoidance();

            if (currentDir != Vector2.zero)
            {
                targetPos = (Vector2)transform.position + currentDir;
                isMoving = true;
            }
            else
            {
                targetPos = transform.position;
                isMoving = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isVulnerable)
            {
                // 1. 通知管理器：我要被吃掉了，请在起点重生我
                PacManGameManager gm = FindObjectOfType<PacManGameManager>();
                if (gm != null)
                    gm.RequeueGhostRespawn(ghostIndex, startPos);

                // 2. 销毁自己
                Destroy(gameObject);
            }
            else
            {
                PacManGameManager gm = FindObjectOfType<PacManGameManager>();
                if (gm != null)
                    gm.GameOver();
            }
        }
    }

    // ---------- 方向辅助方法 ----------
    bool IsValidDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return false;
        Vector2 checkPos = (Vector2)transform.position + dir;
        Collider2D hit = Physics2D.OverlapPoint(checkPos, LayerMask.GetMask("Wall"));
        return hit == null;
    }

    bool IsCellOccupiedByGhost(Vector2 cellPos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(cellPos);
        foreach (var hit in hits)
        {
            GhostGridMover other = hit.GetComponent<GhostGridMover>();
            if (other != null && other != this) return true;
        }
        return false;
    }

    Vector2 FindAvailableDirectionWithGhostAvoidance()
    {
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 dir in dirs)
        {
            if (dir == -currentDir) continue;
            if (IsValidDirection(dir) && !IsCellOccupiedByGhost((Vector2)transform.position + dir))
                return dir;
        }
        foreach (Vector2 dir in dirs)
        {
            if (IsValidDirection(dir) && !IsCellOccupiedByGhost((Vector2)transform.position + dir))
                return dir;
        }
        return Vector2.zero;
    }

    Vector2 DecideDirection()
    {
        if (player == null) return Vector2.left;
        Vector2 desiredDir = isVulnerable ?
            ((Vector2)transform.position - (Vector2)player.position).normalized :
            ((Vector2)player.position - (Vector2)transform.position).normalized;

        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 bestDir = Vector2.left;
        float bestDot = -1f;
        foreach (Vector2 dir in dirs)
        {
            float dot = Vector2.Dot(desiredDir, dir);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestDir = dir;
            }
        }
        return bestDir;
    }

    Vector2 FindAnyValidDirection()
    {
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 dir in dirs)
            if (IsValidDirection(dir)) return dir;
        return Vector2.zero;
    }

    public void MakeVulnerable()
    {
        isVulnerable = true;
        if (spriteRenderer != null)
        {
            if (vulnerableSprite != null)
                spriteRenderer.sprite = vulnerableSprite;
            else
                spriteRenderer.color = Color.blue;
        }
    }

    public void MakeNormal()
    {
        isVulnerable = false;
        if (spriteRenderer != null)
        {
            if (normalSprite != null)
                spriteRenderer.sprite = normalSprite;
            else
                spriteRenderer.color = Color.red;
        }
    }

    // 提供给管理器用于重生后的初始化
    public void InitializeGhost(Vector2 spawnPos, int index)
    {
        transform.position = spawnPos;
        startPos = spawnPos;
        ghostIndex = index;
        AlignToGrid();
        targetPos = transform.position;
        isMoving = false;
        MakeNormal();               // 恢复普通状态
        currentDir = FindAnyValidDirection();
        intendedDir = currentDir;
        if (currentDir != Vector2.zero)
        {
            targetPos = (Vector2)transform.position + currentDir;
            isMoving = true;
        }
    }
}