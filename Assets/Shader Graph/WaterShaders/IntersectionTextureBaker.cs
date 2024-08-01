using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


[RequireComponent(typeof(MeshRenderer))]
public class IntersectionTextureBaker : MonoBehaviour
{
    public enum Resolution
    {
        _256 = 0,
        _512 = 1,
        _1024 = 2,
        _2048 = 3
    }

    public Resolution intersectionTextureResolution;

    public bool useBaked = false;

    public GameObject camPrefab;
    private MeshRenderer meshRenderer;
    private RenderTexture intersectionRT;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        float orthoSize = Mathf.Max(meshRenderer.bounds.size.x, meshRenderer.bounds.size.z) * 0.5f;
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(materialPropertyBlock);

        if (!useBaked)
        {
            GameObject camObject = Instantiate(camPrefab);
            Camera cam = camObject.GetComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = orthoSize;
            cam.transform.eulerAngles = new Vector3(90.0f, 0.0f,0.0f);
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0.0f, 0.05f, 0.0f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0,0,0,0);

            int resolution = 256 << (int)intersectionTextureResolution;
            intersectionRT = new RenderTexture(resolution,resolution,0);
            cam.targetTexture = intersectionRT;
            //if(sdfTexture == null)
            //materialPropertyBlock.SetTexture("_IntersectionTexture", intersectionRT);
            //else
             //   materialPropertyBlock.SetTexture("_IntersectionTexture", sdfTexture);
           
        }

        Shader.SetGlobalVector("_IntersectionCamProperties", new Vector4(transform.position.x,transform.position.y,transform.position.z,orthoSize));
        //materialPropertyBlock.SetVector("_IntersectionCamProperties", new Vector4(transform.position.x,transform.position.y,transform.position.z,orthoSize));
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }


    #if UNITY_EDITOR
        [ContextMenu("Bake intersection texture")]
        public void BakeIntersectionTexture()
        {
            int resolution = 256 << (int)intersectionTextureResolution;
            Texture2D texture = new Texture2D(resolution, resolution);
            RenderTexture.active = intersectionRT;
            texture.ReadPixels(new Rect(0,0,resolution,resolution),0,0);
            texture.Apply();
            RenderTexture.active = null;

            var path = EditorUtility.SaveFilePanel("Save texture as PNG", "Assets", "IntersectionTexture.png", "png");

            if (path.Length != 0)
            {
                byte[] bytes = texture.EncodeToPNG();
                if(bytes != null)
                    File.WriteAllBytes(path, bytes);

                AssetDatabase.Refresh();
            }
        }
    #endif
}
