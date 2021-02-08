using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddGlyphUI : MonoBehaviour
{
    public Sprite defaultCellSprite;
    public GameObject newGlyphSlot;
    public GameObject newGlyphCell;
    private List<List<GameObject>> abilityGrid = new List<List<GameObject>>();
    private PlayerAbility curAbility;
    private Glyph glyphToAdd;
    private void Awake()
    {
        GameObject abilityGridParent = transform.Find("AbilityGrid").gameObject;
        for (int i = 0; i < 7; i++)
        {
            abilityGrid.Add(new List<GameObject>());
        }
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                abilityGrid[j].Add(abilityGridParent.transform.GetChild(j + (i * 7)).gameObject);
            }
        }
    }
    public void Show(PlayerAbility ability, Glyph glyphToAdd)
    {
        curAbility = ability;
        this.glyphToAdd = glyphToAdd;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < abilityGrid.Count; i++)
        {
            for (int j = 0; j < abilityGrid[i].Count; j++)
            {
                if(ability.grid[i][j] != null)
                {
                    abilityGrid[i][j].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1); // sets alpha to 1
                    abilityGrid[i][j].transform.GetChild(0).GetComponent<Image>().sprite = ability.grid[i][j].sprite;
                }
                else
                {
                    abilityGrid[i][j].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0); // sets alpha to 0
                }
            }
        }
        newGlyphSlot.GetComponent<Image>().sprite = glyphToAdd.sprite;
    }
    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false); 
        }
        newGlyphSlot.GetComponent<RectTransform>().anchoredPosition = newGlyphCell.GetComponent<RectTransform>().anchoredPosition;
    }
    public void PlacedNewGlyph(int abilityGridIndex)
    {
        int xIndex = abilityGridIndex % 7;
        int yIndex = Mathf.FloorToInt(abilityGridIndex / 7.0f);
        curAbility.grid[xIndex][yIndex] = Instantiate(glyphToAdd);
    }
}
