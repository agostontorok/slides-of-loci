using UnityEngine;
using System.Collections;

// Script by IJM: http://answers.unity3d.com/questions/29741/mouse-look-script.html
// Changed to fit standard C# conventions
 
// MouseLook rotates the transform based on the mouse delta.
// Minimum and Maximum values can be used to constrain the possible localRotation
 
// To make an FPS style character:
// - Create a capsule.
// - Add a rigid body to the capsule
// - Add the MouseLook script to the capsule.
// -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
// - Add FPSWalker script to the capsule
 
/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
/// -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	 
	public float minimumX = -360F;
	public float maximumX = 360F;
	 
	public float minimumY = -60F;
	public float maximumY = 60F;
	 
	float localRotationX = 0F;
	float localRotationY = 0F;
	 
	Quaternion originallocalRotation;
	 
	void Update()
	{
			// Read the mouse input axis
			localRotationX += Input.GetAxis("Mouse X") * sensitivityX;
			localRotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			 
			localRotationX = ClampAngle(localRotationX, minimumX, maximumX);
			localRotationY = ClampAngle(localRotationY, minimumY, maximumY);
			 
			Quaternion xQuaternion = Quaternion.AngleAxis(localRotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis(localRotationY, -Vector3.right);
			 
			transform.localRotation = originallocalRotation * xQuaternion * yQuaternion;
	}
	 
	void OnEnable()
	{
		// Make the rigid body not change localRotation
		//if (GetComponent<Rigidbody>())
		//	GetComponent<Rigidbody>().freezelocalRotation = true;
		originallocalRotation = transform.localRotation;
	}

	void OnDisable()
	{
		// Make the rigid body not change localRotation
		//if (GetComponent<Rigidbody>())
		//	GetComponent<Rigidbody>().freezelocalRotation = true;
		//originallocalRotation = transform.localRotation;
		localRotationX = 0F;
		localRotationY = 0F;
	}
	 
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}