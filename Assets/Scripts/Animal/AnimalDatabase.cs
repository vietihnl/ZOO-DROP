using UnityEngine;

[CreateAssetMenu(fileName="AnimalDatabase", menuName = "AnimalDrops/Animal Database")]
public class AnimalDatabase : ScriptableObject
{
    [System.Serializable]
    public class AnimalInfo
    {
        [SerializeField] private string animalName;
        [SerializeField] private Sprite sprite;
        [SerializeField] private float diameter = 1f;//đường kính hiển thị
        [SerializeField] private int score;

        public string AnimalName => animalName;
        public Sprite Sprite => sprite;
        public float Diameter => diameter;
        public int Score => score;
    }

    [Range(0.7f, 1f)]
    [SerializeField] private float colliderRatio =0.95f;

    [SerializeField] private AnimalInfo[] animals;

    public float ColliderRatio => colliderRatio;

    public int Count => animals.Length;

    public AnimalInfo Get(int level) => animals[level];
}
