using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public void Awake()
    {
        Sound.LoadBgm("Tutorial", "Tutorial_BGM");
        Sound.LoadBgm("Stage1",      "Stage1BGM");
        Sound.LoadBgm("Stage2", "Stage2BGM");
        Sound.LoadBgm("Stage3", "W3_BGM");

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
        Sound.LoadSe("StageSelect",   "StageSelect");
        Sound.LoadSe("TitleDecision", "TitleDecision");
        Sound.LoadSe("Stan", "Stan");
        Sound.LoadSe("CheckPoint", "CheckPoint");
        Sound.LoadSe("Landing", "Landing");
        Sound.LoadSe("TitleOnButton1", "TitleOnButton1");
        Sound.LoadSe("ScreenUp", "ScreenUp");
        Sound.LoadSe("Spring", "Spring");

        Sound.LoadSe("Creak", "Creak");
        Sound.LoadSe("TutorialClear", "StageDecision2");

    }
}
