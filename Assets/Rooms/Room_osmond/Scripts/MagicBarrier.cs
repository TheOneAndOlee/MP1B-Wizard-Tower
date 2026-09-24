using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MagicBarrier : MonoBehaviour
{
    public UnityEvent enableBook;
    
    [SerializeField] private FrostBook frostBook;

    private Material _mat;
    private Renderer _renderer;
    private Collider _barrierCollider;
    private bool _isFading = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _barrierCollider = GetComponent<Collider>();

        if (_renderer != null)
        {
            _mat = _renderer.material;
        }
    }

    void Awake()
    {
        // StartCoroutine(FadeOut(3));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MagicNullifier") && !_isFading)
        {
            StartCoroutine(FadeOut(3));
        }
    }
    
    private IEnumerator FadeOut(float duration)
    {
        _isFading = true;

        if (_barrierCollider != null)
        {
            _barrierCollider.enabled = false;
        }

        if (_mat == null)
        {
            enableBook.Invoke();

            if (frostBook != null)
            {
                frostBook.EnableFunctionality();
            }

            Destroy(gameObject);
            yield break;
        }

        float currentTime = 0f;
    
        // Read the starting alpha directly from the custom Shader Graph property
        float startAlpha = _mat.HasProperty("_Alpha") ? _mat.GetFloat("_Alpha") : 1f;

        while (currentTime < duration)
        {
            
            float t = currentTime / duration;

            // Smoothly interpolate the float from starting alpha to 0
            float currentAlpha = Mathf.Lerp(startAlpha, 0f, t);

            Debug.Log(currentAlpha);
            
            _mat.SetFloat("_Alpha", currentAlpha);

            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it finishes completely invisible
        _mat.SetFloat("_Alpha", 0f);

        enableBook.Invoke();

        if (frostBook != null)
        {
            frostBook.EnableFunctionality();
        }

        Destroy(gameObject);
    }
}