using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FACinemachineFOVChanger : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float newFOV = 90f;  // Desired new FOV when player enters the trigger

    public float originalFOV;
    public bool isIn = false;

    private void Start()
    {
        if (virtualCamera != null)
        {
            originalFOV = virtualCamera.m_Lens.FieldOfView;  // Store the original FOV
        }
        else
        {
            Debug.LogError("Virtual camera not assigned to CinemachineFOVChanger.");
        }
    }

    public void ChangeFOV()
    {
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.FieldOfView = newFOV;  // Change to the new FOV
        }
    }

    public void ResetFOV()
    {
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.FieldOfView = originalFOV;  // Reset to the original FOV
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter triggered with: " + other.gameObject.name);
        isIn = true;
        if (other.CompareTag("Player"))  // Ensure your player GameObject has the "Player" tag
        {
            ChangeFOV();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit triggered with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            ResetFOV();
        }
    }

}
