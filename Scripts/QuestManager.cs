using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;
    private Dictionary<int, QuestData> questList;
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("모두와 이야기 나누기", new int[]{1000, 1100, 1200}));
        questList.Add(20, new QuestData("호수 조사하기", new int[]{300, 1000}));
        questList.Add(30, new QuestData("산 조사하기", new int[]{1000, 500, 600, 1200}));
        questList.Add(40, new QuestData( "게임끗!", new int[]{0} ));
    }

    public int GetQuestTalkIndex( int NPCId )
    {
        return questId + questActionIndex;
    }

    public string CheckQuest( int id )
    {
        //퀘스트의 한 액션이 끝났을 때 인덱스 증가
        if( id == questList[questId].npcId[questActionIndex] )
            questActionIndex++;
        
        //퀘스트 오브젝트 컨트롤
        ControlObject();
        
        if( questActionIndex == questList[questId].npcId.Length )
            NextQuest();

        return questList[questId].questTitle;
    }
    public string CheckQuest()
    {
        return questList[questId].questTitle;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObject()
    {
        switch ( questId ) {
            case 10:
                if (questActionIndex == 2) {
                    questObject[0].SetActive(true);
                }

                break;
            case 20:
                if (questActionIndex == 0) {
                    questObject[0].SetActive(true);
                }
                if (questActionIndex == 1) {
                    questObject[0].SetActive(false);
                }

                if (questActionIndex == 2)
                {
                    questObject[1].SetActive(true);
                    questObject[2].SetActive(true);
                }

                break;
            case 30:
                if (questActionIndex == 0)
                {
                    questObject[1].SetActive(true);
                    questObject[2].SetActive(true);
                }
                if (questActionIndex == 2)
                {
                    questObject[1].SetActive(false);
                }
                else if (questActionIndex == 3)
                {
                    questObject[2].SetActive(false);
                }

                break;
        }
    }
}
