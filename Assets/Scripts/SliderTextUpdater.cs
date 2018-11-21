using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderTextUpdater : MonoBehaviour {
    public TextMeshProUGUI text;

    public void UpdateText(float value) {
        text.text = Mathf.Round(value * 100f) / 100f + "";
    }
}