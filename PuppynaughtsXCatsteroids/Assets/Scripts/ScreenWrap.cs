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
	bool left;
	bool up;

	public Rigidbody2D rb2D;

	public bool useUnstick;

	float lastX;
	float lastY;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		renderer = GetComponent<MeshRenderer> ();
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
		/*foreach (MeshRenderer mr in renderers) {
			if (mr.isVisible) {
				return true;
			}
		}*/
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

			lastX = transform.position.x;
			if (lastX < 0) {
				left = true;
			} else {
				left = false;
			}

			lastY = transform.position.y;
			if (lastY > 0) {
				up = true;
			} else {
				up = false;
			}

			return;
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
			if (rb2D.velocity.y < -15) {
				rb2D.velocity = new Vector2 (rb2D.velocity.x, -15);
			}
			isWrappingY = true;
		}

		transform.position = newPosition;

		if(useUnstick){
			StartCoroutine ("UnStick");
		}

	}

	public void RendUpdate(){
		renderer = GetComponent<MeshRenderer> ();
	}

	IEnumerator UnStick(){
		bool stuck = true;
		float timer = 0;
		while (stuck) {
			if (timer >= .05f) {
				Debug.Log ("Unsticking");
				if (left) {
					transform.position = new Vector3 (lastX + .1f, transform.position.y, 0f);
					stuck = false;
				} else {
					transform.position = new Vector3 (lastX - .1f, transform.position.y, 0f);
					stuck = false;
				}
				if (up) {
					transform.position = new Vector3 (transform.position.x, lastY-.1f, 0f);
					stuck = false;
				} else {
					transform.position = new Vector3 (transform.position.x, lastY*-1, 0f);
					stuck = false;
				}
			}
			/*foreach (MeshRenderer mr in renderers) {
				if (mr.isVisible) {
					stuck = false;
					break;
				}
			}*/
			if (renderer.isVisible) {
				stuck = false;
			}


			timer += Time.deltaTime;
			yield return new WaitForSeconds (.1f);
		}
		yield break;
	}
}
