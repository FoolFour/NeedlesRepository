using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public void Awake()
    {
        Sound.LoadBgm("Stage1", "Stage1BGM");
        Sound.LoadBgm("Title", "TitleBGM");
        Sound.LoadBgm("StageSelect", "StageSelectBGM");

        Sound.LoadSe("BlockBreak", "BlockBreak");
        Sound.LoadSe("Grip", "Grip");
        Sound.LoadSe("NonPickBlock", "NonPickBlock");
        Sound.LoadSe("Pick", "Pick");
        Sound.LoadSe("StageClear", "StageClear");
        Sound.LoadSe("StageDecision", "StageDecision");
    }
}
