using UnityEngine;
using UnityEngine.UI;

public class NextAnimalUI : MonoBehaviour
{
    [SerializeField] private AnimalSpawner spawner;
    [SerializeField] private AnimalDatabase database;
    [SerializeField] private Image previewImage;

    private void OnEnable()
    {
        spawner.NextLevelChanged += OnNextLevelChanged;
    }

    private void OnDisable()
    {
        spawner.NextLevelChanged -= OnNextLevelChanged;
    }

    private void OnNextLevelChanged(int level)
    {
        previewImage.sprite = database.Get(level).Sprite;
        previewImage.preserveAspect = true;
    }
}
