using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;

    private Material material;
    private Vector2 offset;

    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (GameManager.instance.GameState == GameState.GameOver)
            return;

        offset.x += scrollSpeed * Time.deltaTime;
        material.mainTextureOffset = offset; 
    }

}
