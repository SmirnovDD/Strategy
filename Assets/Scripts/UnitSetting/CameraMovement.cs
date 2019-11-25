using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public VirtualRotateJoystick rotateJoystick;
    public VirtualJoystick moveJoystick;
    public Transform lookPointX;
    public float movementSpeed;
    public float rotationSpeed;

    private float x, y;
    private float perspectiveZoomSpeed = 0.05f;

    private bool registerRotationTouch;
    private bool checkingRotationTouchCourIsRunning;

    private bool rotationTouchWasFirst;
    private bool isRotating;

    private bool setTouchPhaseManualyToBegan;
    private void Start()
    {
        Vector3 euler = transform.eulerAngles;
        x = euler.y;
        y = euler.z;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameController.battleEnded)
            return;

        MoveCamera();

        if(GameController.battleStarted)
            RotateCamera();

        PinchToZoom();
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
                if (!checkingRotationTouchCourIsRunning)
                    StartCoroutine(RotationTouchRegistrationDelay());
            }
            else if ((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary) && registerRotationTouch)
            {
                transform.Rotate(Vector3.up, rotateJoystick.InputDirection.x * rotationSpeed * Time.deltaTime, Space.World);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                rotateJoystick.startTouch = Vector3.zero;
                rotateJoystick.joystickImg.rectTransform.anchoredPosition = Vector3.zero;
                rotateJoystick.InputDirection = Vector3.zero;
                //rotateJoystick.gameObject.SetActive(false);
                registerRotationTouch = false;
                rotationTouchWasFirst = false;

                isRotating = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            if (!rotationTouchWasFirst)
            {
                if (moveJoystick.isMoving && !setTouchPhaseManualyToBegan)
                {
                    //rotateJoystick.gameObject.SetActive(true);
                    setTouchPhaseManualyToBegan = true;
                    rotateJoystick.gameObject.transform.position = (Vector3)Input.GetTouch(1).position - (Vector3.right * rotateJoystick.bgImg.rectTransform.sizeDelta.x / 2 * rotateJoystick.scaleFactor)
                                                                    + (Vector3.down * rotateJoystick.bgImg.rectTransform.sizeDelta.y / 2 * rotateJoystick.scaleFactor);
                    rotateJoystick.startTouch = Input.GetTouch(1).position;

                    isRotating = true;
                }
                else if (moveJoystick.isMoving && (Input.GetTouch(1).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Stationary))
                {
                    transform.Rotate(Vector3.up, rotateJoystick.InputDirection.x * rotationSpeed * Time.deltaTime, Space.World);
                }
                else if (Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Canceled)
                {
                    rotateJoystick.startTouch = Vector3.zero;
                    rotateJoystick.joystickImg.rectTransform.anchoredPosition = Vector3.zero;
                    rotateJoystick.InputDirection = Vector3.zero;
                    //rotateJoystick.gameObject.SetActive(false);

                    setTouchPhaseManualyToBegan = false;
                    isRotating = false;
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (!checkingRotationTouchCourIsRunning)
                        StartCoroutine(RotationTouchRegistrationDelay());
                }
                else if ((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary) && registerRotationTouch)
                {
                    transform.Rotate(Vector3.up, rotateJoystick.InputDirection.x * rotationSpeed * Time.deltaTime, Space.World);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    rotateJoystick.startTouch = Vector3.zero;
                    rotateJoystick.joystickImg.rectTransform.anchoredPosition = Vector3.zero;
                    rotateJoystick.InputDirection = Vector3.zero;
                    //rotateJoystick.gameObject.SetActive(false);
                    registerRotationTouch = false;
                    rotationTouchWasFirst = false;

                    isRotating = false;
                }
            }
            
        }
        else
        {
            rotateJoystick.startTouch = Vector3.zero;
            rotateJoystick.joystickImg.rectTransform.anchoredPosition = Vector3.zero;
            rotateJoystick.InputDirection = Vector3.zero;
            rotateJoystick.transform.position = rotateJoystick.defaultPos;
            registerRotationTouch = false;
            isRotating = false;
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

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -10, 10), transform.position.y, Mathf.Clamp(transform.position.z, -16.71f, 16.71f));
    }

    public void PinchToZoom()
    {
        if (Input.touchCount == 2 && !moveJoystick.isMoving && !isRotating)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            // Otherwise change the field of view based on the change in distance between the touches.
            Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

            // Clamp the field of view to make sure it's between 0 and 180.
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30f, 90f);
        }
    }

    private IEnumerator RotationTouchRegistrationDelay()
    {
        checkingRotationTouchCourIsRunning = true;
        yield return new WaitForSeconds(0.05f);

        if(Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            isRotating = true;
            rotationTouchWasFirst = true;
            rotateJoystick.touchBeganOutsideUI = true;

            //rotateJoystick.gameObject.SetActive(true);
            rotateJoystick.gameObject.transform.position = (Vector3)Input.GetTouch(0).position - (Vector3.right * rotateJoystick.bgImg.rectTransform.sizeDelta.x / 2 * rotateJoystick.scaleFactor)
                                                            + (Vector3.down * rotateJoystick.bgImg.rectTransform.sizeDelta.y / 2 * rotateJoystick.scaleFactor);
            rotateJoystick.startTouch = Input.GetTouch(0).position;

            registerRotationTouch = true;
        }
        checkingRotationTouchCourIsRunning = false;
    }
}