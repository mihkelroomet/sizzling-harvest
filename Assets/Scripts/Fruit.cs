using UnityEngine;

public class Fruit : Clickable
{
    // Components
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;
    private Rigidbody2D _rb;
    public SpriteRenderer WhiteSpriteRenderer;

    // Collecting
    private bool _beingCollected;
    private bool _fading;
    private float _startedTurningWhiteAt;
    private float _startToFadeAt;
    private float _destructAt; // Also used in burning
    public float TurnWhiteDuration;
    public float FadeDuration;

    // Burning
    private bool _burning;
    private bool _flickering;
    private float _startedBurningAt;
    private float _startToFlickerAt;
    public float BurnDuration;
    public float FlickerDuration;

    public FruitData Data;

    public AudioClipGroup CollectAudio;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _beingCollected = false;
        _burning = false;
        _flickering = false;
    }

    private void Start()
    {
        _spriteRenderer.sprite = Data.Sprite;
        WhiteSpriteRenderer.sprite = Data.WhiteSprite;
        _rb.gravityScale = Data.GravityScale;
    }

    private void Update()
    {
        if (_beingCollected)
        {
            float a;

            if (_fading)
            {
                if (_destructAt <= Time.time)
                {
                    Destroy(gameObject);
                    return;
                }

                // Fade away
                a = (_destructAt - Time.time) / FadeDuration;
                WhiteSpriteRenderer.color = new Color(1, 1, 1, a);
                return;
            }

            if (_startToFadeAt <= Time.time)
            {
                _fading = true;
                _spriteRenderer.color = new Color(1, 1, 1, 0);
                return;
            }

            // Turn white
            a = (Time.time - _startedTurningWhiteAt) / TurnWhiteDuration;
            WhiteSpriteRenderer.color = new Color(1, 1, 1, a);
            return;
        }

        if (_burning)
        {
            if (_flickering)
            {
                if (_destructAt <= Time.time)
                {
                    Destroy(gameObject);
                    return;
                }

                // Flicker
                _spriteRenderer.color = new Color(0, 0, 0, Mathf.Abs(_spriteRenderer.color.a - 1));
                return;
            }

            if (_startToFlickerAt <= Time.time)
            {
                _flickering = true;
                return;
            }

            // Fade to black
            float whiteness = (_startToFlickerAt - Time.time) / BurnDuration;
            _spriteRenderer.color = new Color(whiteness, whiteness, whiteness, 1);
        }
    }

    /// <summary>
    /// Collect fruit if it's not yet burning
    /// </summary>
    public void OnMouseDown()
    {
        if (!_burning && GameController.Instance.GameRunning)
        {
            _beingCollected = true;
            _collider.isTrigger = true;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.velocity = Vector2.zero;
            _rb.freezeRotation = true;
            _startedTurningWhiteAt = Time.time;
            _startToFadeAt = Time.time + TurnWhiteDuration;
            _destructAt = _startToFadeAt + FadeDuration;

            CollectAudio.Play();

            Events.SetScore(Events.GetScore() + Data.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire") && !_beingCollected)
        {
            Burn();
        }
    }

    private void Burn()
    {
        _burning = true;
        _startedBurningAt = Time.time;
        _startToFlickerAt = Time.time + BurnDuration;
        _destructAt = _startToFlickerAt + FlickerDuration;
    }
}
