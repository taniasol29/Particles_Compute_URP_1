using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class S0_tintChanger : MonoBehaviour
{
    private Bloom _bloom;
    private Volume volume;

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out _bloom);

        StartCoroutine(changeTint());
    }

    IEnumerator changeTint()
    {
        var myTint = _bloom.tint.GetValue<Color>();

        while (true)
        { 
            ChangeTintValue(myTint);
            yield return new WaitForSeconds(1);
            ChangeTintValue(myTint);
        }
    }

    void ChangeTintValue(Color c)
    {
        c = GetRandomTint();
        _bloom.tint.value = c;
    }

    private Color GetRandomTint()
    {
        float r = Random.Range(0.0f, 255.0f);
        float g = Random.Range(0.0f, 255.0f);
        float b = Random.Range(0.0f, 255.0f);

        return new Color(r, g, b);
    }
}
