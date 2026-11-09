using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private AnimalDatabase database;
    [SerializeField] private Animal animalPrefab;
    [SerializeField] private ParticleSystem mergeFxPrefab;

    public event System.Action<int, Vector3> Merged;
   
    private int score;
    private int highestLevel;

    public event System.Action<int> HighestLevelChanged;
    public int HighestLevel=> highestLevel;

    public event System.Action<int> ScoreChanged;
    public int Score => score;

    private void OnEnable()
    {
        Animal.MergeRequested += HandleMerge;
    }

    private void OnDisable()
    {
        Animal.MergeRequested -= HandleMerge;
    }

    private void HandleMerge(Animal a, Animal b)
    {
        if(a==null || b==null) return;
        if(a.IsMerged || b.IsMerged) return;

        int nextLevel = a.Level + 1;
        if(nextLevel >= database.Count) return;

        a.MarkMerged();
        b.MarkMerged();

        Vector3 position = (a.transform.position + b.transform.position)*0.5f;

        Destroy(a.gameObject);
        Destroy(b.gameObject);

        Animal merged = Instantiate(animalPrefab, position, Quaternion.identity);
        merged.Init(database, nextLevel);
        merged.Drop();
        merged.SuppressHitSound();

        merged.PlayPop();
        SpawnFx(position,nextLevel);
        Merged?.Invoke(nextLevel, position);

        score += database.Get(nextLevel).Score;
        Debug.Log("Score: "+score);
        ScoreChanged?.Invoke(score);

        if(nextLevel > highestLevel)
        {
            highestLevel = nextLevel;
            HighestLevelChanged?.Invoke(highestLevel);
        }
    }
    private void SpawnFx(Vector3 position, int level)
    {
        if(mergeFxPrefab == null) return;

        ParticleSystem fx = Instantiate(mergeFxPrefab, position, Quaternion.identity);
        fx.transform.localScale = Vector3.one*database.Get(level).Diameter;
    }
}
