using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private RectTransform rect;
    private int idx;
    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private GameObject tileGenerator;
    private GameObject monsterManager;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        slot1 = GameObject.Find("OfferSlot1");
        slot2 = GameObject.Find("OfferSlot2");
        slot3 = GameObject.Find("OfferSlot3");
        tileGenerator = GameObject.Find("TileGenerator");
        monsterManager = GameObject.Find("MonsterManager");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject tile;
        if(slot1.transform.childCount != 0)
        {
            tile = slot1.transform.GetChild(0).gameObject;
        } else if(slot2.transform.childCount != 0)
        {
            tile = slot2.transform.GetChild(0).gameObject;
        } else
        {
            tile = slot3.transform.GetChild(0).gameObject;
        }
        

        if(transform.childCount == 0)
        {
            tile.transform.SetParent(transform);
            tile.GetComponent<RectTransform>().position = rect.position;

            int type = tile.GetComponent<TileDraggable>().tileType;
            BoardCheck.adj[idx / 5 + 1, idx % 5 + 1] = type;
            tileGenerator.GetComponent<BoardCheck>().displayedTileCount += 1;
            tile.GetComponent<TileDraggable>().enabled = false;

            tileGenerator.GetComponent<TileGenerator>().MinusTileCount();
            tileGenerator.GetComponent<BoardCheck>().Check();

            SoundManager.Instance.PlayDisplaySound();
        } 
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            int type = eventData.pointerDrag.GetComponent<TileDraggable>().tileType;
            if ((monsterManager.GetComponent<MonsterSpawner>().CheckMonster() == 2 || (monsterManager.GetComponent<MonsterSpawner>().CheckMonster() == 3 && monsterManager.GetComponent<MonsterSpawner>().buckShotMode == 2)) && ((idx == 0 && type == 9) || (idx == 4 && type == 3) || (idx == 20 && type == 12) || (idx == 24 && type == 6)))
            {
                image.color = Color.red;
            }
            else
            {
                image.color = Color.yellow;
            }
        }
        else
        {
            image.color = Color.yellow;
        }
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
