using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private Dictionary<int, string[]> dialogue;
    private Dictionary<int, Sprite> portData;
    public Sprite[] portArr;
    
    void Awake()
    {
        dialogue = new Dictionary<int, string[]>();
        portData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        dialogue.Add(1000, new string[] {"앗, 안녕하세요!#2",
            "아하하, 예상대로네!\n역시 당신이 한 거였군요?#2"});
        dialogue.Add(1100, new string[] {"에에.....#0",
            "여, 여긴 또 어디야....#2",
            "저...저기, 저기.. 에이 씨...?\n여, 여긴 어디예요..?#2",
            "....나도 몰라요!#-98",
            "다, 당신이 모르면 누가 안다는 거예요..!#2",
            "ㄱ..그럼, 저 애는 누구예요..?\n어라? 날개? 링? 천사..??#2",
            "아하, 저 애는 괜찮아요! 그냥..\n개발자가 좋아하는 캐릭터라서 넣고 싶었대요!#-98",
            "에에......?#1",
            "캐릭터 붕괴의 위험성이 있어서,\n저 애는 여기서 아무것도 안 할 거니까 안심해요.#-100",
            "에엑...........#2"});
        dialogue.Add(1200, new string[] {"...............#0"});
        dialogue.Add(100, new string[]{"상자다.",
            "...이런 것까지 일일히 대화창 띄울 필요 있어?"});
        dialogue.Add(101, new string[]{"책상이다. 위에 뭐라고 쓰여 있지만, 굉장한 악필이다.\n전혀 읽을 수가 없다..",
            "하지만 왜 이런 곳에 책상이 있는 걸까?"});
        
        //A
        portData.Add( 1000+0, portArr[0] );
        portData.Add( 1000+1, portArr[1] );
        portData.Add( 1000+2, portArr[2] );
        portData.Add( 1000+3, portArr[3] );
        
        //Lupert
        portData.Add( 1100+0, portArr[4] );
        portData.Add( 1100+1, portArr[5] );
        portData.Add( 1100+2, portArr[6] );
        portData.Add( 1100+3, portArr[7] );
        
        //Angel
        portData.Add( 1200+0, portArr[8] );
        portData.Add( 1200+1, portArr[9] );
    }

    public string GetTalk( int id, int talkIndex )
    {
        if (talkIndex == dialogue[id].Length)//넘겨받은 인덱스가 전체 길이와 같을 때(데이터가 더 이상 없을 때)
            return null;
        else
            return dialogue[id][talkIndex];
    }

    public Sprite GetPortrait( int id, int portIndex )
    {
        return portData[id + portIndex];
    }
}
