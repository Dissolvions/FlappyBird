using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.1f;
    [SerializeField] private float floatSpeed = 4f;

    [Header("Movement")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    private Rigidbody2D rg;
    private Vector3 startPosition;
    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        startPosition = Vector3.zero;
        rg.simulated = false;
    }

    void Update()
    {
        if(GameManager.instance.GameState == GameState.Home || GameManager.instance.GameState == GameState.GetReady)
        {
            FloatIdle();
        }
        else
        {
            RotateBird();
        }
    }

    private void FloatIdle()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void RotateBird()
    {
        float angle = Mathf.Clamp(rg.linearVelocityY * 5f, -90f, 30f);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            rotationSpeed * Time.deltaTime
        );
    }

    public void OnSpace(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (GameManager.instance.GameState == GameState.Home || GameManager.instance.GameState == GameState.GameOver) return;

        if(GameManager.instance.GameState == GameState.GetReady)
        {
            GameManager.instance.GamePlay();
            rg.simulated = true;
        }

        rg.linearVelocity = new Vector2(rg.linearVelocity.x, 0f);
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        AudioManager.instance.Fly();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.GameState == GameState.GameOver) return;
        if (!collision.collider.CompareTag("Obstacle"))
            return;

        AudioManager.instance.Hit();
        StartCoroutine(DieSoundDeLay());

        GameManager.instance.GameOver();
    }

    IEnumerator DieSoundDeLay()
    {
        yield return new WaitForSeconds(0.3f);
        AudioManager.instance.Die();
    }
    public void ResetPlayer()
    {
        rg.simulated = false;
        rg.linearVelocity = Vector2.zero;
        rg.angularVelocity = 0f;
        startPosition = new Vector3(-0.5f, 0f, 0f);
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
    }

  
}