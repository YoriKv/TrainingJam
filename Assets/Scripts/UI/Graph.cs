using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Graph:MonoBehaviour {
    public LayoutElement gridBarPrefab;

    private int _numBars;
    private float _gridWidth;
    private float _gridHeight;
    private List<LayoutElement> _gridBars;

    public void Start() {
        // Init sizes, has to be in start so rect transform has a chance to calculate
        RectTransform rt = GetComponent<RectTransform>();
        _gridWidth = rt.rect.size.x;
        _gridHeight = rt.sizeDelta.y;
        _numBars = Mathf.RoundToInt(_gridWidth / 15f);
        _gridBars = new List<LayoutElement>(_numBars);

        for(int i = 0; i < _numBars; i++) {
            LayoutElement bar = Instantiate(gridBarPrefab);
            bar.transform.SetParent(transform, false);
            bar.minHeight = 0f;
            _gridBars.Add(bar);
        }

#if UNITY_EDITOR
        StartCoroutine(Blah());
#endif
    }

#if UNITY_EDITOR
    private IEnumerator Blah() {
        while(isActiveAndEnabled) {
            SetValue(Random.value, false);
            yield return new WaitForSeconds(0.1f);
        }
    }
#endif

    public void SetValue(float value, bool special) {
        LayoutElement bar = _gridBars[0];
        // Set new value
        bar.GetComponent<Image>().color = special ? Color.red : Color.green;
        bar.minHeight = value * _gridHeight;
        // Push to end of transform
        bar.transform.SetAsLastSibling();
        // Move to end of list
        _gridBars.RemoveAt(0);
        _gridBars.Add(bar);
    }
}
