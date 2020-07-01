using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementObject : MonoBehaviour
{
    [SerializeField]
    private bool IsSelected;

    [SerializeField]
    private bool IsLocked;

    public bool Selected
    {
        get
        {
            return this.IsSelected;
        }
        set
        {
            IsSelected = value;
        }
    }

    public bool Locked
    {
        get
        {
            return this.IsLocked;
        }
        set
        {
            IsLocked = value;
        }
    }

    [SerializeField]
    private TextMeshPro OverlayText;

    [SerializeField]
    private GameObject moveIcon;

    [SerializeField]
    private string OverlayDisplayText;

    public GameObject ring;
    public GameObject objectPlace;

    private GameObject spawnRing;

    public void SetOverlayText(string text)
    {
        if (OverlayText != null)
        {
            OverlayText.gameObject.SetActive(true);
            OverlayText.text = text;
        }
    }

    public void SetMoveIcon(bool move)
    {
        if(move)
        {
            moveIcon.gameObject.SetActive(true);
        }

        if(!move)
        {
            moveIcon.gameObject.SetActive(false);
            spawnRing = Instantiate(ring, objectPlace.transform.position, objectPlace.transform.rotation);

        }
        
    }

    void Awake()
    {
        OverlayText = GetComponentInChildren<TextMeshPro>();
        if (OverlayText != null)
        {
            OverlayText.gameObject.SetActive(false);
        }
    }

    public void ToggleOverlay()
    {
        OverlayText.gameObject.SetActive(IsSelected);
        OverlayText.text = OverlayDisplayText;
    }

   
}