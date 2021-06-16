using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questTitle;
    public int[] npcId;

    public QuestData( string questTitle, int[] npcId )
    {
        this.questTitle = questTitle;
        this.npcId = npcId;
    }
}
