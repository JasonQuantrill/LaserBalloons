using UnityEngine;
using Valve.VR;

public class Laser : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // Hand type (left or right)
    public SteamVR_Action_Boolean laserAction; // Action to trigger the laser
    public LineRenderer laserLineRenderer; // Assign in the inspector
    public float laserMaxLength = 5f; // Maximum length of the laser

    private void Update()
    {
        if (laserAction.GetState(handType))
        {
            ShootLaserFromController();
        }
        else
        {
            laserLineRenderer.enabled = false;
        }
    }

    void ShootLaserFromController()
    {
        RaycastHit hit;
        laserLineRenderer.enabled = true;
        laserLineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit, laserMaxLength))
        {
            laserLineRenderer.SetPosition(1, hit.point);
            Destroy(hit.collider.gameObject); // Destroy the object hit by the laser
        }
        else
        {
            laserLineRenderer.SetPosition(1, transform.position + transform.forward * laserMaxLength);
        }
    }
}



