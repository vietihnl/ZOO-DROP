using UnityEngine;
using UnityEngine.UI;

public class EvolutionBarUI : MonoBehaviour
{
    [SerializeField] private AnimalDatabase database;
    [SerializeField] private RectTransform container;
    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private float iconSize = 50f;

    [SerializeField] private bool hideLocked = false;
    [SerializeField] private Color lockedColor = new Color(0f, 0f, 0f, 0.35f);

    private Image[] icons;

    private void OnEnable()
    {
        mergeManager.HighestLevelChanged += Refresh;
    }

    private void OnDisable()
    {
        mergeManager.HighestLevelChanged -= Refresh;
    }

    private void Start()
    {
        Build();
        Refresh(mergeManager.HighestLevel);
    }

    private void Build()
    {
        int count = database.Count;
        float step = container.rect.width / count;
        icons = new Image[count];

        for(int i = 0; i < count; i++)
        {
            GameObject go = new GameObject("Icon_"+i, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(container,false);

            RectTransform rt= go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0,0.5f);
            rt.anchorMax = new Vector2(0f,0.5f);
            rt.pivot = new Vector2(0.5f,0.5f);
            rt.sizeDelta = new Vector2(iconSize, iconSize);
            rt.anchoredPosition = new Vector2(step * (i+0.5f),0f);

            Image img = go.GetComponent<Image>();
            img.sprite = database.Get(i).Sprite;
            img.preserveAspect = true;
            img.raycastTarget = false;

            icons[i] = img;
        }
    }

    private void Refresh(int highestLevel)
    {
        if(icons == null) return;

        for(int i=0; i<icons.Length; i++)
        {
            bool unlocked = i <= highestLevel;

            icons[i].enabled = unlocked || !hideLocked;
            icons[i].color = unlocked? Color.white : lockedColor;
        }
    }
}
