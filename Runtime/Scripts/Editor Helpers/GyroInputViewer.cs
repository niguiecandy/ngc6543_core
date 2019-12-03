using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace NGC6543.DebugHelper
{
	public class GyroInputViewer : MonoBehaviour 
	{
		[Header("Gyro", order=0)]

		[Header("Attitude", order=1)]
		public Text attitudeX;
		public Text attitudeY;
		public Text attitudeZ;
		public RectTransform rotDial_AttX, rotDial_AttY, rotDial_AttZ;

		[Header("RotationRate")]
		public Text rotRateX;
		public Text rotRateY;
		public Text rotRateZ;
		public RectTransform vertSlider_rotRX, vertSlider_rotRY, vertSlider_rotRZ;
		[Range(1f, 5f)]public float rotRSliderScale = 2f;

		[Header("RotationRateUnbiased")]
		public Text rotRateXUnbiased;
		public Text rotRateYUnbiased;
		public Text rotRateZUnbiased;
		public RectTransform vertSlider_rotRUX, vertSlider_rotRUY, vertSlider_rotRUZ;
		[Range(1f, 5f)]public float rotRUSliderScale = 2f;

		[Header("UserAcceleration")]
		public Text userAccX;
		public Text userAccY;
		public Text userAccZ;
		public RectTransform vertSlider_userAccX, vertSlider_userAccY, vertSlider_userAccZ;
		[Range(1f, 5f)]public float userAccSliderScale = 2f;

		bool _gyroSupported = true;
		Gyroscope gyro;

		void OnEnable()
		{
			if (!SystemInfo.supportsGyroscope)
			{
				Debug.LogWarning("The system doesn't support gyroscope!");
				return;
			}
			gyro = Input.gyro;
		}

		// Update is called once per frame
		void Update () 
		{
			if (!SystemInfo.supportsGyroscope) return;
			
			
			// attitude
			var v = gyro.attitude.eulerAngles;
			attitudeX.text = System.Math.Round(v.x,3).ToString();
			attitudeY.text = System.Math.Round(v.y,3).ToString();
			attitudeZ.text = System.Math.Round(v.z,3).ToString();
			rotDial_AttX.eulerAngles = new Vector3(0,0,v.x);
			rotDial_AttY.eulerAngles = new Vector3(0,0,v.y);
			rotDial_AttZ.eulerAngles = new Vector3(0,0,v.y);

			// rotationRate
			v = gyro.rotationRate;
			rotRateX.text = System.Math.Round(v.x,3).ToString();
			rotRateY.text = System.Math.Round(v.y,3).ToString();
			rotRateZ.text = System.Math.Round(v.z,3).ToString();
			vertSlider_rotRX.localPosition = new Vector3(0,rotRSliderScale * v.x, 0);
			vertSlider_rotRY.localPosition = new Vector3(0,rotRSliderScale * v.y, 0);
			vertSlider_rotRZ.localPosition = new Vector3(0,rotRSliderScale * v.z, 0);

			// rotationRateUnbiased
			v = gyro.rotationRateUnbiased;
			rotRateXUnbiased.text = System.Math.Round(v.x,3).ToString();
			rotRateYUnbiased.text = System.Math.Round(v.y,3).ToString();
			rotRateZUnbiased.text = System.Math.Round(v.z,3).ToString();
			vertSlider_rotRUX.localPosition = new Vector3(0,rotRUSliderScale * v.x, 0);
			vertSlider_rotRUY.localPosition = new Vector3(0,rotRUSliderScale * v.y, 0);
			vertSlider_rotRUZ.localPosition = new Vector3(0,rotRUSliderScale * v.z, 0);

			// userAcceleration
			v = gyro.userAcceleration;
			userAccX.text = System.Math.Round(v.x,3).ToString();
			userAccY.text = System.Math.Round(v.y,3).ToString();
			userAccZ.text = System.Math.Round(v.z,3).ToString();
			vertSlider_userAccX.localPosition = new Vector3(0,userAccSliderScale * v.x, 0);
			vertSlider_userAccY.localPosition = new Vector3(0,userAccSliderScale * v.y, 0);
			vertSlider_userAccZ.localPosition = new Vector3(0,userAccSliderScale * v.z, 0);
			
		}
	}
}