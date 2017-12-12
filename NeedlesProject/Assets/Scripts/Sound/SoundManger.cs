using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public void Awake()
    {
        Sound.LoadBgm("Stage1",      "Stage1BGM");
        Sound.LoadBgm("Title",       "TitleBGM");
        Sound.LoadBgm("StageSelect", "StageSelectBGM");

        Sound.LoadSe("BlockBreak",    "BlockBreak");
        Sound.LoadSe("Grip",          "Grip");
        Sound.LoadSe("NonPickBlock",  "NonPickBlock");
        Sound.LoadSe("Pick",          "Pick");
        Sound.LoadSe("StageClear",    "StageClear");
        Sound.LoadSe("StageDecision", "StageDecision");
        Sound.LoadSe("Extend",        "Extend");
        Sound.LoadSe("Swish",         "Swish");
        Sound.LoadSe("ArmBreak",      "ArmBreak");
        Sound.LoadSe("Explosion",     "Explosion");
        Sound.LoadSe("CursorMove",    "CursorMove");
        Sound.LoadSe("MenuClose",     "MenuClose");
        Sound.LoadSe("MenuDecision",  "MenuDecision");
        Sound.LoadSe("MenuOpen",      "MenuOpen");
        Sound.LoadSe("StageSelect",    "StageSelect");
        Sound.LoadSe("TitleDecision", "TitleDecision");
    }
}
