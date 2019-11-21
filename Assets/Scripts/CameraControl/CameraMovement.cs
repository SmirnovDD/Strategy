using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public VirtualRotateJoystick rotateJoystick;
    public Transform lookPointX;
    public float movementSpeed;
    public float rotationSpeed;

    private float x, y;

    private void Start()
    {
        Vector3 euler = transform.eulerAngles;
        x = euler.y;
        y = euler.z;
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    public void RotateCamera()
    {
//#if UNITY_EDITOR
//        if (Input.GetKey(KeyCode.RightArrow))
//        {
//            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
//        }
//        else if(Input.GetKey(KeyCode.LeftArrow))
//        {
//            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
//        }
//#elif UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                rotateJoystick.gameObject.SetActive(true);
                rotateJoystick.gameObject.transform.position = (Vector3)Input.GetTouch(0).position - (Vector3.right * rotateJoystick.bgImg.rectTransform.sizeDelta.x / 2 * rotateJoystick.scaleFactor)
                                                                + (Vector3.down * rotateJoystick.bgImg.rectTransform.sizeDelta.y / 2 * rotateJoystick.scaleFactor);
                rotateJoystick.startTouch = Input.GetTouch(0).position;

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                transform.Rotate(Vector3.up, rotateJoystick.InputDirection.x * rotationSpeed * Time.deltaTime, Space.World);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                rotateJoystick.startTouch = Vector3.zero;
                rotateJoystick.joystickImg.rectTransform.anchoredPosition = Vector3.zero;
                rotateJoystick.InputDirection = Vector3.zero;
                //rotateJoystick.gameObject.SetActive(false);
            }            
        }
        else if(Input.touchCount == 2)
        {

        }
        else
        {
            rotateJoystick.startTouch = Vector3.zero;
            rotateJoystick.joystickImg.rectTransform.anchoredPosition = Vector3.zero;
            rotateJoystick.InputDirection = Vector3.zero;
            rotateJoystick.transform.position = rotateJoystick.defaultPos;
        }
        //#endif
    }
    public void MoveCamera()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            transform.Translate((VirtualJoystick.InputDirection * movementSpeed * Time.deltaTime), Space.Self);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            transform.Translate((VirtualJoystick.InputDirection * movementSpeed * Time.deltaTime), Space.Self);
        }
#endif
    }
}
