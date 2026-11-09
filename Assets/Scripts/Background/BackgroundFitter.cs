using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitter : MonoBehaviour
{
    SpriteRenderer sr;
    Camera cam;
    int lastW, lastH;

    void OnEnable(){Fit();}

    void Update()
    {
        if(Screen.width != lastW|| Screen.height != lastH) Fit();

    }

    void Fit()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if(cam == null) cam = Camera.main;
        lastW=Screen.width;
        lastH=Screen.height;

        float camH= cam.orthographicSize*2f;
        float camW= camH*cam.aspect;
        Vector2 spriteSize= sr.sprite.bounds.size; //kích thước sprite theo unit

        //lấy hệ số lớn phủ 2 chiều
        float  scale = Mathf.Max(camW/spriteSize.x, camH/spriteSize.y);

        transform.localScale= new Vector3(scale, scale, 1);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y,0f);
    }
}
