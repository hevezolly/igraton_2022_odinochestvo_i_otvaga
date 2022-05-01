using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicFloor : MonoBehaviour, IFloorProvider
{
    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private UnityEvent onFloorUpdated;
    [SerializeField]
    private bool canBeWiped;

    private Rect rect;

    private RenderTexture renderTex;
    private Texture2D tex;

    public bool CanBeWiped => canBeWiped;

    public void SetWipable()
    {
        canBeWiped = true;
        rect = GetLocalRect();
    }

    private void Awake()
    {
        tex = CreateTexture();
        renderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2f, renderer.sprite.pixelsPerUnit);
        GetLocalRect();
        renderTex = CreateRenderTexture();
    }

    private Texture2D CreateTexture()
    {
        var tex = new Texture2D(renderer.sprite.texture.width, renderer.sprite.texture.height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        for (var x = 0; x < renderer.sprite.texture.width; x++)
        {
            for (var y = 0; y < renderer.sprite.texture.height; y++)
            {
                tex.SetPixel(x, y, renderer.sprite.texture.GetPixel(x, y));
            }
        }
        tex.Apply();
        return tex;
    }

    private RenderTexture CreateRenderTexture()
    {
        var tex = new RenderTexture(renderer.sprite.texture.width, renderer.sprite.texture.height, 0);
        tex.enableRandomWrite = true;
        var a = RenderTexture.active;
        RenderTexture.active = tex;
        Graphics.Blit(renderer.sprite.texture, tex);
        RenderTexture.active = a;
        return tex;
    }

    public Rect GetLocalRect()
    {
        var widht = renderer.sprite.texture.width / (float)renderer.sprite.pixelsPerUnit;
        var height = renderer.sprite.texture.height / (float)renderer.sprite.pixelsPerUnit;
        rect = new Rect(-new Vector2(widht, height) / 2, new Vector2(widht, height));
        return rect;
    }
    

    public RenderTexture GetTexture()
    {
        return renderTex;
    }

    private IEnumerator FillTexture()
    {
        yield return new WaitForEndOfFrame();
        var a = RenderTexture.active;
        RenderTexture.active = renderTex;
        tex.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0, false);
        tex.Apply();
        RenderTexture.active = a;
        //var s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2f, renderer.sprite.pixelsPerUnit);
        //renderer.sprite = s;

    }

    public void UpdateTexture()
    {
        StartCoroutine(FillTexture());
        onFloorUpdated?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        var r = GetLocalRect();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(r.center, r.size);
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
