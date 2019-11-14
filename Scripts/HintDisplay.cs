using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintDisplay : MonoBehaviour {

    TextMeshProUGUI hint;
    TextMeshProUGUI hintShadow;

    public string[] hints;


    private void Start()
    {

    }


    public void ChangeHint(int num)
    {
        hint = transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        hintShadow = gameObject.GetComponent<TextMeshProUGUI>();
        hint.text = hints[num];
        hint.color = new Color(1f, 1f, 1f, 0f);
        hintShadow.text = hints[num];
        hintShadow.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(HintFade());
    }

    IEnumerator HintFade()
    {

        for (float i = 0f; i < 2f; i += Time.deltaTime)
        {
            if (i < 1f)
            {
                hint.color = new Color(1f, 1f, 1f, i);
                hintShadow.color = new Color(0f, 0f, 0f, i);
            }
            else
            {
                hint.color = new Color(1f, 1f, 1f, 1f);
                hintShadow.color = new Color(0f, 0f, 0f, 1f);
            }
            yield return null;
        }
        for (float i = 2f; i > 0f; i -= Time.deltaTime)
        {
            if (i > 1f)
            {
                hint.color = new Color(1f, 1f, 1f, 1);
                hintShadow.color = new Color(0f, 0f, 0f, 1);
            }
            else
            {
                hint.color = new Color(1f, 1f, 1f, i);
                hintShadow.color = new Color(0f, 0f, 0f, i);
            }
            yield return null;
        }
        hint.color = new Color(1f, 1f, 1f, 0f);
        hintShadow.color = new Color(0f, 0f, 0f, 0f);
        Destroy(gameObject);
    }
}
