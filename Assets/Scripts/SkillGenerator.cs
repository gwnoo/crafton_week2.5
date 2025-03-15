using UnityEngine;

public class SkillGenerator : MonoBehaviour
{
    public GameObject[] Skill;
    public int skillCount = 0;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }


    public void CastSkill(int index)
    {
        if (index == 1)
        {
            if (transform.GetChild(0).childCount > 0)
            {
                int skill = transform.GetChild(0).GetChild(0).GetComponent<SkillIcon>().skillType;
                player.GetComponent<PlayerController>().CastSkill(skill);
            }
            DeleteSkill(transform.GetChild(0));
            MoveSkill(transform.GetChild(0), transform.GetChild(1));
            MoveSkill(transform.GetChild(1), transform.GetChild(2));
        }
        else if (index == 2)
        {
            if (transform.GetChild(1).childCount > 0)
            {
                int skill = transform.GetChild(1).GetChild(0).GetComponent<SkillIcon>().skillType;
                player.GetComponent<PlayerController>().CastSkill(skill);
            }
            DeleteSkill(transform.GetChild(1));
            MoveSkill(transform.GetChild(1), transform.GetChild(2));
        }
        else if (index == 3)
        {
            if (transform.GetChild(2).childCount > 0)
            {
                int skill = transform.GetChild(2).GetChild(0).GetComponent<SkillIcon>().skillType;
                player.GetComponent<PlayerController>().CastSkill(skill);
            }
            DeleteSkill(transform.GetChild(2));
        }
    }
    
    public void Generate(int index)
    {
        if (skillCount == 0)
        {
            SkillGenerate(index, transform.GetChild(0));
        }
        else if (skillCount == 1)
        {
            SkillGenerate(index, transform.GetChild(1));
        }
        else if (skillCount == 2)
        {
            SkillGenerate(index, transform.GetChild(2));
        }
        else if (skillCount == 3)
        {
            DeleteSkill(transform.GetChild(0));
            MoveSkill(transform.GetChild(0), transform.GetChild(1));
            MoveSkill(transform.GetChild(1), transform.GetChild(2));
            SkillGenerate(index, transform.GetChild(2));
        }
    }

    private void SkillGenerate(int index, Transform slot)
    {
        GameObject newTile = Instantiate(Skill[index], slot);
        newTile.transform.SetParent(slot);
        skillCount += 1;
    }

    private void DeleteSkill(Transform slot)
    {
        Transform tile;

        if (slot.childCount > 0)
        {
            tile = slot.GetChild(0);
            Destroy(tile.gameObject);
            skillCount -= 1;
        }
    }

    private void MoveSkill(Transform slot1, Transform slot2)
    {
        GameObject tile;

        if (slot2.childCount > 0)
        {
            tile = slot2.GetChild(0).gameObject;

            tile.transform.SetParent(slot1);
            tile.GetComponent<RectTransform>().position = slot1.gameObject.GetComponent<RectTransform>().position;
        }
    }
}
