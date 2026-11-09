using UnityEngine;
using System.Collections;

public class AnimalTester : MonoBehaviour
{
    [SerializeField] private AnimalDatabase database;
    [SerializeField] private Animal animalPrefab;
    [SerializeField] private float spawnY = 6f;
    [SerializeField] private float interval = 0.6f;

    private IEnumerator Start()
    {
        for(int i=0; i< database.Count; i++)
        {
            Animal f = Instantiate(animalPrefab,
            new Vector3(Random.Range(-2f,2f), spawnY, 0f), Quaternion.identity);
            f.Init(database,i);
            f.Drop();
            yield return new WaitForSeconds(interval);
        }
    }
}
