using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private RectTransform rect;
    private int idx;
    private GameObject slot;
    private GameObject tileGenerator;
    public int tileType;
    private KeyCode[] keyCodes = {
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
    };

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        slot = GameObject.Find("OfferSlot1");
        tileGenerator = GameObject.Find("TileGenerator");
    }

    private void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) && i == idx)
            {
                if(transform.childCount > 0)
                {
                    break;
                }
                GameObject tile = slot.transform.GetChild(0).gameObject;
                tile.transform.SetParent(transform);
                tile.GetComponent<RectTransform>().position = rect.position;
                //tile.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;

                int type = tile.GetComponent<TileDraggable>().tileType;
                BoardCheck.adj[idx / 3 + 1, idx % 3 + 1] = type;
                tile.GetComponent<TileDraggable>().enabled = false;

                tileGenerator.GetComponent<TileGenerator>().MinusTileCount();
                tileGenerator.GetComponent<BoardCheck>().Check();

                SoundManager.Instance.PlayDisplaySound();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        GameObject tile = slot.transform.GetChild(0).gameObject;
        tile.transform.SetParent(transform);
        tile.GetComponent<RectTransform>().position = rect.position;
        //tile.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;

        int type = tile.GetComponent<TileDraggable>().tileType;
        BoardCheck.adj[idx / 3 + 1, idx % 3 + 1] = type;
        tile.GetComponent<TileDraggable>().enabled = false;

        tileGenerator.GetComponent<TileGenerator>().MinusTileCount();
        tileGenerator.GetComponent<BoardCheck>().Check();

        SoundManager.Instance.PlayDisplaySound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
        }
    }

    

    public void SetIdx(int x)
    {
        idx = x;
    }

    public int GetIdx()
    {
        return idx;
    }
}
