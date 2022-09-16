using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{
    public int Arrows = 5;
    public TMPro.TextMeshProUGUI ArrowsText;
    public int Score = 3;
    public TMPro.TextMeshProUGUI ScoreText;

    public TrailRenderer Trail;
    public GameObject ArrowImage;
    public GameObject ArrowPrefab;

    [SerializeField] Rigidbody2D _rb = null;
    [SerializeField] AudioSource _audio = null;
    [SerializeField] GameObject _gameOverPopup = null;
    public TMPro.TextMeshProUGUI _gameoverScoreText = null;
    [SerializeField] AudioSource _win = null;
    [SerializeField] AudioSource _lose = null;

    int _scored = 0;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _rb.isKinematic = true;
        _rb.gravityScale = 0f;
        Trail.enabled = false;
        ArrowsText.text = "Flechas: " + Arrows;
        ScoreText.text = $"Puntuación: {_scored}/{Score}";
        _gameOverPopup.SetActive(false);
    }

    public void Shoot(float force)
    {
        _rb.gravityScale = 1f;
        _rb.isKinematic = false;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.AddForce(transform.right * force, ForceMode2D.Impulse);
        Trail.enabled = true;
        --Arrows;
        ArrowsText.text = "Flechas: " + Arrows;
        ScoreText.text = $"Puntuación: {_scored}/{Score}";
        _audio.Play();
    }

    public bool Drag()
    {
        if(!_rb.isKinematic || Arrows < 1)
        {
            return false;
        }
        transform.SetParent(null, true);
        ArrowImage.SetActive(true);
        Disable();
        return true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(20f);
        }

        if(!_rb.isKinematic)
        {
            transform.right = _rb.velocity.normalized;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("positive"))
        {
            Debug.Log("Good");
            Instantiate(ArrowPrefab, ArrowImage.transform.position, ArrowImage.transform.rotation).transform.SetParent(collision.transform, true);
            ArrowImage.SetActive(false);
            Disable();
            ++_scored;
        }
        else if (collision.CompareTag("negative"))
        {
            Debug.Log("Bad");
            Instantiate(ArrowPrefab, ArrowImage.transform.position, ArrowImage.transform.rotation).transform.SetParent(collision.transform, true);
            ArrowImage.SetActive(false);
            Disable();
            _scored = Mathf.Max(0, _scored-1);
        }
        else if (collision.CompareTag("neutral"))
        {
            Debug.Log("Meh");
            Instantiate(ArrowPrefab, ArrowImage.transform.position, ArrowImage.transform.rotation).transform.SetParent(collision.transform, true);
            ArrowImage.SetActive(false);
            Disable();
        }
        transform.position = Vector3.one * 1000f;
        ArrowsText.text = "Flechas: " + Arrows;
        ScoreText.text = $"Puntuación: {_scored}/{Score}";
        collision.GetComponent<Target>()?.Hit();
        if(Arrows < 1 || _scored >= Score)
        {
            _gameOverPopup.SetActive(true);
            _gameoverScoreText.text = $"Has adertado {_scored}/{Score}";
            if(_scored >= Score)
            { 
                _win.Play(); 
            }
            else
            {
                _lose.Play();
            }
        }
    }

    void Disable()
    {
        _rb.isKinematic = true;
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        Trail.enabled = false;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
