using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private GameObject interactUI;

    private StationInteract currentStation;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            StationInteract station = hit.collider.GetComponent<StationInteract>();

            if (station != null)
            {
                currentStation = station;

                if (interactUI != null)
                    interactUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentStation.Interact();
                }

                return;
            }
        }

        currentStation = null;

        if (interactUI != null)
            interactUI.SetActive(false);
    }
}
