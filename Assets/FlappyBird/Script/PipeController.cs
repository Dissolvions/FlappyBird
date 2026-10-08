using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float hideXPosition = -10f;
    private PipeSpawner spawner;
    private ScoreZone scoreZone;


    void Awake()
    {
        scoreZone = GetComponentInChildren<ScoreZone>();
    }

    void OnEnable()
    {
        scoreZone.resetscoreZone();
    }
    private void Update()
    {
        if (GameManager.instance.GameState != GameState.Playing)
            return;
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if(transform.position.x < hideXPosition)
        {
            spawner.ReturnToPool(this);
        }
    }
    public void Initialize(PipeSpawner pipeSpawner)
    {
        spawner = pipeSpawner;

    }
}
