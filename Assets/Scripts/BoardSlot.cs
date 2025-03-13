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

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        slot = GameObject.Find("OfferSlot1");
        tileGenerator = GameObject.Find("TileGenerator");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject tile = slot.transform.GetChild(0).gameObject;
        tile.transform.SetParent(transform);
        tile.GetComponent<RectTransform>().position = rect.position;
        //tile.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;

        int type = tile.GetComponent<TileDraggable>().tileType;
        BoardCheck.adj[idx / 5 + 1, idx % 5 + 1] = type;
        tileGenerator.GetComponent<BoardCheck>().displayedTileCount += 1;
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
