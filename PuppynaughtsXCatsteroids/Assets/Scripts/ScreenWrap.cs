using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

	//Screen wrap variables
	private MeshRenderer renderer;
	private Camera cam;
	private Vector3 viewportsPosition;
	public bool isWrappingX;
	public bool isWrappingY;

	public bool startOffScreen;

	public Rigidbody2D rb2D;

	float lastX;
	float lastY;

	// Use this for initialization
	void Start () {
		cam = Camera.main;

		if (!GetComponent<MeshRenderer> ()) {
			renderer = GetComponentInChildren<MeshRenderer> ();
		} else {
			renderer = GetComponent<MeshRenderer> ();
		}

		viewportsPosition = cam.WorldToViewportPoint (transform.position);
		isWrappingX = false;
		isWrappingY = false;
		rb2D = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (startOffScreen) {
			if (CheckRenderers ()) {
				startOffScreen = false;
			}
		} else {
			ScreenWrapFunct ();
		}
	}

	//Screen Wrap functions
	bool CheckRenderers(){
		
		if (renderer.isVisible) {
			return true;
		}

		return false;
	}

	void ScreenWrapFunct(){
		bool isVisible = CheckRenderers();

		if (isVisible) {
			isWrappingX = false;
			isWrappingY = false;
		}

		if (isWrappingX && isWrappingY) {
			return;
		}

		viewportsPosition = cam.WorldToViewportPoint (transform.position);
		Vector3 newPosition = transform.position;



		if (!isWrappingX && (viewportsPosition.x > 1 || viewportsPosition.x < 0)) {


			newPosition.x = -newPosition.x;
			isWrappingX = true;

		}



		if (!isWrappingY && (viewportsPosition.y > 1 || viewportsPosition.y < 0)) {

			newPosition.y = -newPosition.y;
			isWrappingY = true;
		}

		transform.position = newPosition;


	}

	public void RendUpdate(){
		renderer = GetComponent<MeshRenderer> ();
	}


}
