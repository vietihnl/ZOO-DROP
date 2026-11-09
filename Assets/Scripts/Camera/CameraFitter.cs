using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraFitter : MonoBehaviour
{
    [SerializeField] SpriteRenderer background;


    Camera cam;
    float lastAspect;

    void OnEnable(){Fit();}
    void LateUpdate()
    {
        if(cam == null) cam = GetComponent<Camera>();
        if(!Mathf.Approximately(cam.aspect, lastAspect)) Fit();
    }

    void Fit()
    {
        if(cam==null) cam = GetComponent<Camera>();
        if(background ==null) return;

        lastAspect = cam.aspect;

        Vector2 size= background.bounds.size; //lích thước ảnh theo unit
        float sizeByHeight = size.y*0.5f; //fit chiều cao
        float sizeByWidth = size.x / (2f*cam.aspect);

        //lấy số lớn
        cam.orthographicSize =  Mathf.Max(sizeByHeight, sizeByWidth);
        Vector3 c = background.bounds.center;
        transform.position = new Vector3(c.x, c.y, -10f);
    }
}
