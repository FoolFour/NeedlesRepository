using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageData : MonoBehaviour
{
	public delegate void UpdateData();
	public event UpdateData OnUpdateData;

	private string stageName_;
	public  string stageName      { get { return stageName_;      } }
	public  void   ApplyStageName(string stageName) 
	{
		stageName_ = stageName;
		SendUpdateEvent();
	}

	private string sceneName_;
	public  string sceneName      { get { return sceneName_;      } }
	public  void   ApplySceneName(string sceneName) 
	{
		sceneName_ = sceneName;
		SendUpdateEvent();
	}

	private float  time_;
	public  float  time           { get { return time_;           } }
	public  void   ApplyTime(float time) 
	{
		time_ = time;
		SendUpdateEvent();
	}

	private float  border1_;
	public  float  border1        { get { return border1_;        } }
	public  void   ApplyBorder1(float border1) 
	{
		border1_ = border1;
		SendUpdateEvent();
	}

	private float  border2_;
	public  float  border2        { get { return border2_;        } }
	public  void   ApplyBorder2(float border2) 
	{
		border2_ = border2;
		SendUpdateEvent();
	}



	private bool   isNewRecord_;
	public  bool   isNewRecord    { get { return isNewRecord_;    } }
	public  void   ApplyIsNewRecord(bool isNewRecord) 
	{
		isNewRecord_ = isNewRecord;
		SendUpdateEvent();
	}

	private bool   isBorder1Clear_;
	public  bool   isBorder1Clear { get { return isBorder1Clear_; } }
	public  void   ApplyIsBorder1Clear(bool isBorder1Clear) 
	{
		isBorder1Clear_ = isBorder1Clear;
		SendUpdateEvent();
	}

	private bool   isBorder2Clear_;
	public  bool   isBorder2Clear { get { return isBorder2Clear_; } }
	public  void   ApplyIsBorder2Clear(bool isBorder2Clear) 
	{
		isBorder2Clear_ = isBorder2Clear;
		SendUpdateEvent();
	}


	void SendUpdateEvent()
	{
		if(OnUpdateData == null) { return; }
		OnUpdateData();
	}
}
