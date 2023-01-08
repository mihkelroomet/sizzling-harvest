using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner Instance;
    public bool Active {get; set;}
    public float FrameSpawnProbability {get; set;} // Probability for each frame whether a fruit will spawn or not
    public float MinDelay; // Minimum delay between fruits
    public float MaxDelay; // Maximum delay between fruits
    private float _nextSpawnFrom;

    // Spawn area
    public float SpawnY;
    public float MinSpawnX;
    public float MaxSpawnX;

    public Fruit Fruit;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Active = false;
        FrameSpawnProbability = 2.25f;
    }

    private void Update()
    {
        if (Active && _nextSpawnFrom <= Time.time && (Time.time > _nextSpawnFrom + MaxDelay || Random.value < FrameSpawnProbability * Time.deltaTime))
        {
            Fruit fruit = Instantiate(Fruit, new Vector3(Random.Range(MinSpawnX, MaxSpawnX), SpawnY, 0), Quaternion.identity);
            fruit.transform.parent = transform;
            _nextSpawnFrom = Time.time + MinDelay;
        }
    }

    public void ResetFrameSpawnProbability()
    {
        FrameSpawnProbability = 2.25f;
    }
}
