using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateWithSlider : MonoBehaviour
{
    public RotateObject _canal;
	private Slider _slider;

    public void Start()
    {
        _slider = transform.gameObject.GetComponent<Slider>();
		_slider.onValueChanged.AddListener(call: delegate { OnValueChanged(); });
	}

    private void OnValueChanged()
    {
        var angle = _slider.value * 180 + _canal.transform.eulerAngles.y;
        _canal.Rotate(Vector3.down, angle);
    }
}
