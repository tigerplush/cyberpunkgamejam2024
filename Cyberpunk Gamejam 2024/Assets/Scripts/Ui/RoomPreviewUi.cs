using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPreviewUi : MonoBehaviour
{
    [SerializeField]
    private Image[] _iconImages;
    [SerializeField]
    private Sprite _recruitmentSprite;

    public void BuildRoomPreview(RoomConfiguration config)
    {
        if(config.RoomType == RoomType.RecruitmentRoom)
        {
            _iconImages[0].sprite = _recruitmentSprite;
            _iconImages[0].gameObject.SetActive(true);
            return;
        }
        for(int i = 0; i < config.OffensiveEnemies.Count; i++)
        {
            _iconImages[i].sprite = config.OffensiveEnemies[i].PossibleElements[0].Icon;
            _iconImages[i].color = config.OffensiveEnemies[i].PossibleElements[0].PrimaryColor;
            _iconImages[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < config.DefensiveEnemies.Count; i++)
        {
            _iconImages[i + 2].sprite = config.DefensiveEnemies[i].PossibleElements[0].Icon;
            _iconImages[i + 2].color = config.DefensiveEnemies[i].PossibleElements[0].PrimaryColor;
            _iconImages[i + 2].gameObject.SetActive(true);
        }
    }
}
