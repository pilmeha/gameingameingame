using UnityEngine;

public class PacmanGridMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite openMouthSprite;      // 张嘴图片（嘴朝左）
    public Sprite closedMouthSprite;    // 闭嘴图片（嘴朝左）
    public float animationInterval = 0.2f; // 动画切换间隔（秒）

    private Vector2 targetPos;
    private Vector2 currentDir = Vector2.left;
    private Vector2 nextDir = Vector2.left;
    private Rigidbody2D rb;
    private bool isMoving = false;

    private SpriteRenderer spriteRenderer;
    private float animationTimer = 0f;
    private bool isOpenMouth = true;    // 当前是否显示张嘴图片

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("Pacman需要SpriteRenderer组件！");

        // 初始位置对齐网格
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        targetPos = transform.position;
        currentDir = Vector2.left;
        nextDir = currentDir;
        isMoving = false;

        // 设置初始动画和方向
        UpdateSpriteAndRotation();
    }

    void Update()
    {
        // 1. 方向输入
        if (Input.GetKeyDown(KeyCode.UpArrow)) nextDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) nextDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) nextDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) nextDir = Vector2.right;

        // 2. 网格移动逻辑
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            transform.position = targetPos;
            isMoving = false;

            if (IsValidDirection(nextDir))
                currentDir = nextDir;
            else if (!IsValidDirection(currentDir))
                currentDir = Vector2.zero;

            if (currentDir != Vector2.zero)
            {
                if (IsValidDirection(currentDir))
                {
                    targetPos = (Vector2)transform.position + currentDir;
                    isMoving = true;
                }
            }
            else
            {
                targetPos = transform.position;
                isMoving = false;
            }

            // 方向改变时更新旋转角度
            UpdateSpriteAndRotation();
        }

        // 3. 动画：周期性切换张嘴/闭嘴
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationInterval)
        {
            animationTimer = 0f;
            isOpenMouth = !isOpenMouth;
            UpdateSpriteAndRotation();
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
            rb.MovePosition(Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime));
    }

    bool IsValidDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return false;
        Vector2 checkPos = (Vector2)transform.position + dir;
        Collider2D hit = Physics2D.OverlapPoint(checkPos, LayerMask.GetMask("Wall"));
        return hit == null;
    }

    // 更新精灵图片以及根据移动方向旋转
    void UpdateSpriteAndRotation()
    {
        if (spriteRenderer == null) return;

        // 选择当前要显示的精灵（张嘴或闭嘴）
        Sprite targetSprite = isOpenMouth ? openMouthSprite : closedMouthSprite;
        if (targetSprite != null)
            spriteRenderer.sprite = targetSprite;

        // 根据移动方向旋转吃豆人（假设默认图片嘴朝左）
        if (currentDir != Vector2.zero)
        {
            float angle = 0f;
            if (currentDir == Vector2.left) angle = 0f;
            else if (currentDir == Vector2.right) angle = 180f;
            else if (currentDir == Vector2.up) angle = 270f;
            else if (currentDir == Vector2.down) angle = 90f;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pellet"))
        {
            Destroy(other.gameObject);
            FindObjectOfType<PacManGameManager>().AddScore(10);
        }
        else if (other.CompareTag("PowerPellet"))
        {
            Destroy(other.gameObject);
            FindObjectOfType<PacManGameManager>().PowerUp();
        }
        // 幽灵碰撞由幽灵自身的脚本处理，这里不再需要
    }
}