using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MagicBarrier : MonoBehaviour
{
    public UnityEvent enableBook;
    
    [SerializeField] private FrostBook frostBook;

    private Material _mat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Material mat =  frostBook.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SelfDestruct()
    {
        StartCoroutine(FadeOut(2));
    }

    private IEnumerator FadeOut(int duration)
    {
        float currentTime = 0;
        Color startColor = _mat.color;
        Color endColor = new Color(_mat.color.r, _mat.color.g, _mat.color.b, 0);

        while (currentTime < duration)
        {
            _mat.color = Color.Lerp(startColor, endColor, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        enableBook.Invoke();
        Destroy(gameObject);
    }
}
