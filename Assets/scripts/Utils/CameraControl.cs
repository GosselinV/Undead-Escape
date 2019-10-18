using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	const float speed = 10f;
	const int orthographicSizeMin = 1;
	const int orthographicSizeMax = 8;
	Vector3 mouseOrigin = new Vector3();
	float panSpeed = -0.3f;

	void Update(){
		Vector3 camPos = transform.position;
		if (camPos.x < -GameConstants.CurrentMapSizeX / 2f) {
			transform.position = new Vector3 (-GameConstants.CurrentMapSizeX / 2f, camPos.y, camPos.z);
		} else if (camPos.x > GameConstants.CurrentMapSizeX / 2f) {
			transform.position = new Vector3 (GameConstants.CurrentMapSizeX / 2f, camPos.y, camPos.z);
		} else if (camPos.y < -GameConstants.CurrentMapSizeY / 2f) {
			transform.position = new Vector3 (camPos.x, -GameConstants.CurrentMapSizeY / 2f, camPos.z);
		} else if (transform.position.y > GameConstants.CurrentMapSizeY / 2f) {
			transform.position = new Vector3 (camPos.x, GameConstants.CurrentMapSizeY / 2f, camPos.z);
		} else {
			if (Input.GetKey ("w")) {
				transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
			}
			if (Input.GetKey ("a")) {
				transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
			} 
			if (Input.GetKey ("s")) {
				transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
			}
			if (Input.GetKey ("d")) {
				transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
			}
			if (Input.GetMouseButtonDown (1)) {
				mouseOrigin = Input.mousePosition;
			}
			if (Input.GetMouseButton (1)) {
				Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - mouseOrigin).normalized;
				Vector3 move = new Vector3 (pos.x * panSpeed, pos.y * panSpeed, 0);
				transform.Translate (move, Space.Self);
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
			Camera.main.orthographicSize--;
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
			Camera.main.orthographicSize++;
		}

		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
	}


}
