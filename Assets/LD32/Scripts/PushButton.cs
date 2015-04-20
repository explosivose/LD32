using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour {

	public float pushDepth;
	public float cooldown;
	
	private Vector3 _defaultPos;
	private bool _pressed;
	private bool _heldDown;
	
	void Awake() {
		_defaultPos = transform.localPosition;
	}
	
	IEnumerator OnMouseDown() {
		if (_pressed) yield break;
		
		SendMessageUpwards("OnPushButtonDown", SendMessageOptions.DontRequireReceiver);
		_pressed = true;
		_heldDown = true;
		yield return new WaitForSeconds(cooldown);
		_pressed = _heldDown;
	}
	
	void OnMouseUp() {
		SendMessageUpwards("OnPushButtonUp", SendMessageOptions.DontRequireReceiver);
		_heldDown = false;
	}
	
	void Update() {
		Vector3 target = _pressed ? _defaultPos + Vector3.forward * pushDepth : _defaultPos;
		transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 8f);
	}
}
