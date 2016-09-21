using UnityEngine;
using System.Collections;

public class ModelScript : MonoBehaviour {

    public float rotationSpeed = 180.2f;
    public float mouseSensitivity = 0.4f;
    public float rotationDamper;

    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;

    void Update() {
        if (isRotating) {
            mouseOffset = Input.mousePosition - mouseReference;
            rotation.y = (mouseOffset.x + mouseOffset.y) * -mouseSensitivity;
            transform.Rotate(rotation);
            mouseReference = Input.mousePosition;
        } else {
            float sign = rotation.y < 0 ? -1: 1;
            rotation.y -= rotationDamper * sign * Time.deltaTime;
            rotation.y = Mathf.Abs(rotation.y) <= 1 ? sign : rotation.y;
            transform.rotation *= Quaternion.AngleAxis(rotationSpeed * sign, Vector3.up);
        }
    }

    void OnMouseDown() {
        isRotating = true;
        mouseReference = Input.mousePosition;
    }

    void OnMouseUp() {
        isRotating = false;
    }
}
