using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame) // A button on Oculus Controller
                Debug.Log("A Button Pressed!");
            if (Gamepad.current.buttonWest.wasPressedThisFrame) // X button on Oculus Controller
                Debug.Log("X Button Pressed!");
        }
        else
        {
            Debug.Log("No Controller Detected!");
        }
    }
}