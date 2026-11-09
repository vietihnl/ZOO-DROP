using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class AnimalSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnimalDatabase database;
    [SerializeField] private Animal animalPrefab;
    [SerializeField] private Camera cam;

    [Header("Spawn")]
    [SerializeField] private float spawnY= 6f;
    [SerializeField] private float minX=-4f;
    [SerializeField] private float maxX= 4f;
    [SerializeField] private int maxSpawnLevel= 4;
    [SerializeField] private float respawnDelay= 0.5f;

    [Header("Guide Line")]
    [SerializeField] private Sprite dotSprite;
    [SerializeField] private float dotSpacing= 0.45f;
    [SerializeField] private float dotSize= 0.18f;
    [SerializeField] private int maxDots= 40;
    [SerializeField] private LayerMask guideMask = ~0;
    [SerializeField] private GameManager gameManager;

    public event System.Action AnimalDropped;
    private InputAction pressAction;
    private InputAction positionAction;

    private Animal currentAnimal;
    private float currentRadius;
    private int nextLevel;
    private float lastX;
    private bool isPointerDown;
    private SpriteRenderer[] dots;
    public event System.Action<int> NextLevelChanged;

    public int NextLevel => nextLevel;

    private void Awake()
    {
        pressAction= new InputAction("Press", InputActionType.Button, "<Pointer>/press");
        positionAction= new InputAction("Position", InputActionType.Value, "<Pointer>/position");
    }

    private void OnEnable()
    {
        pressAction.Enable();
        positionAction.Enable();
    }

    private void OnDisable()
    {
        pressAction.Disable();
        positionAction.Disable();
    }

    private void OnDestroy()
    {
        pressAction.Dispose();
        positionAction.Dispose();
    }

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        if(cam==null) cam= Camera.main;
        maxSpawnLevel = Mathf.Clamp(maxSpawnLevel, 0, database.Count -1);

        CreateDots();
        nextLevel = RandomLevel();
        SpawnAnimal();
    }

    private void Update()
    {
        if(gameManager.IsGameOver||gameManager.IsPaused)
        {
            isPointerDown = false;
            HideDots();
            return;
        }
        
        if(currentAnimal == null)
        {
            HideDots();
            return;
        }

        if(pressAction.WasPressedThisFrame())
        {
            isPointerDown = true;
        }
        if(isPointerDown&&pressAction.IsPressed())
        {
            MoveAnimalToPointer();
        }

        UpdateGuide();

        if(isPointerDown && pressAction.WasReleasedThisFrame())
        {
            isPointerDown = false;
            DropAnimal();
        }

    }

    private void SpawnAnimal()
    {
        int level = nextLevel;
        nextLevel = RandomLevel();
        NextLevelChanged?.Invoke(nextLevel);

        currentAnimal = Instantiate(animalPrefab, new Vector3(lastX, spawnY, 0f), Quaternion.identity);
        currentAnimal.Init(database, level);
        currentAnimal.Hold();

        currentRadius = database.Get(level).Diameter*0.5f;
        ClampAnimalX(lastX);
    }

    private void DropAnimal()
    {
        currentAnimal.Drop();
        AnimalDropped?.Invoke();
        currentAnimal = null;
        HideDots();
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnAnimal();
    }

    private int RandomLevel()
    {
        return Random.Range(0, maxSpawnLevel+1);
    }

    private void MoveAnimalToPointer()
    {
        Vector2 screenPos = positionAction.ReadValue<Vector2>();
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -cam.transform.position.z));
        ClampAnimalX(world.x);
    }

    private void ClampAnimalX(float x)
    {
        float clamped = Mathf.Clamp(x, minX + currentRadius, maxX - currentRadius);
        lastX = clamped;
        currentAnimal.transform.position = new Vector3(clamped, spawnY, 0f);
    }

    private void CreateDots()
    {
        dots = new SpriteRenderer[maxDots];
        float scale = dotSize / dotSprite.bounds.size.x;

        for(int i = 0; i< maxDots; i++)
        {
            GameObject go = new GameObject("Dot_"+i);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one*scale;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = dotSprite;
            sr.sortingOrder = -1;
            sr.color = new Color(1f, 0.35f, 0.45f, 0.9f);

            go.SetActive(false);
            dots[i] = sr;
        }
    }

    private void UpdateGuide()
    {
        float x = currentAnimal.transform.position.x;
        float castRadius = currentRadius*database.ColliderRatio;

        RaycastHit2D hit = Physics2D.CircleCast(new Vector2(x, spawnY), castRadius, Vector2.down, 30f, guideMask);

        float startY = spawnY - currentRadius - dotSpacing*0.5f;
        float endY = hit.collider !=null
            ? hit.centroid.y - currentRadius
            : startY - 30f;

        int count = Mathf.Min(maxDots, Mathf.FloorToInt((startY - endY)/dotSpacing)+1);

        for(int i = 0; i< maxDots; i++)
        {
            bool visible = i < count;
            GameObject go = dots[i].gameObject;
            if(go.activeSelf != visible) go.SetActive(visible);
            if(visible) go.transform.position = new Vector3(x, startY - i*dotSpacing, 0f);

        }
    }

    private void HideDots()
    {
        if(dots == null) return;
        for(int i=0; i<dots.Length; i++)
        {
            if(dots[i].gameObject.activeSelf) dots[i].gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(minX, spawnY, 0f), new Vector3(maxX, spawnY, 0f));
    }
}
