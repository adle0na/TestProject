using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GoogleSheetManager : Singleton<GoogleSheetManager>
{
	private static GoogleSheetManager instance;
	
	const string URL = "https://docs.google.com/spreadsheets/d/1ekHdkJqHKicKDH4EZaDkkv5D5FZiB_uq8I4RuVznbwY/export?format=tsv";
	//&rang=A2:B2;
	//&gid=5454968
	public bool isDataLoad;

	public List<CharacterData>      characterDB;
	public List<MonsterData>        monsterDB;
	public List<IngredientItem>     ingredientItemDB;
	public List<ConsumableItem>     consumableItemDB;
	public List<WeaponData>         weaponDB;
	public List<ArmorData>          armorDB;
	public List<AllEventData>       allEventDB;
	public List<Event1Data>         event1DB;
	public List<Event2Data>         event2DB;
	public List<Event3Data>         event3DB;
	public List<ShopData>           shopDB;
	public List<MissionData>        missionDB;
	public List<SkillData>          skillDB;
	public List<InAppositeWordData> inAppositeWordDB;

	private Coroutine getDataCor;
	
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		// 모든 씬에서 유지
		DontDestroyOnLoad(gameObject);
	}

	public void LoadAllData()
	{
		Debug.LogError("모든 데이터 조회 실행");

		if (getDataCor != null)
		{
			StopCoroutine(getDataCor);
		}
		getDataCor = StartCoroutine(GetDataCor());
	}

	IEnumerator GetDataCor()
	{
		foreach (TableDataType value in TableDataType.GetValues(typeof(TableDataType)))
		{
			string tableURL = URL + GetTableCodeWithName(value);

			UnityWebRequest www = UnityWebRequest.Get(tableURL);

			yield return www.SendWebRequest();

			string data = www.downloadHandler.text;

			Debug.Log($"{value}\n{data}");

			 switch (value)
			 {
				case TableDataType.Character:
					CharacterDataParse(data);
				break;
				case TableDataType.Monster:
					MonsterDataParse(data);
					break;
				case TableDataType.IngredientItem:
					IngredientItemDataParse(data);
					break;
				case TableDataType.ConsumableItem:
					ConsumableItemDataParse(data);
					break;
				case TableDataType.Weapon:
					//ConsumableItemDataParse(data);
					break;
				case TableDataType.Armor:
					//SelectEventDataParse(data);
					break;
				case TableDataType.AllEvent:
					break;
				case TableDataType.Event1:
					break;
				case TableDataType.Event2:
					break;
				case TableDataType.Event3:
					break;
				case TableDataType.Shop:
					break;
				case TableDataType.Mission:
					break;
				case TableDataType.Skill:
					break;
				case TableDataType.InAppositeWord:
					break;
			 }
		}

		isDataLoad = true;
	}

	string GetTableCodeWithName(TableDataType tableType)
	{
		string header = "&gid=";

		string tableCode = "";
		
		switch (tableType)
		{
			case TableDataType.Character:
				tableCode = "0&range=A4:V";
				break;
			case TableDataType.Monster:
				tableCode = "1154871274&range=A4:N";
				break;
			case TableDataType.IngredientItem:
				tableCode = "1289380474&range=A4:E";
				break;
			case TableDataType.ConsumableItem:
				tableCode = "866585351&range=A4:E";
				break;
			case TableDataType.Weapon:
				//tableCode = "2136310630&range=A4:L";
				break;
			case TableDataType.Armor:
				//tableCode = "270773085&range=A4:D";
				break;
			case TableDataType.AllEvent:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.Event1:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.Event2:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.Event3:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.Shop:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.Skill:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.Mission:
				//tableCode = "1452065059&range=A4:S";
				break;
			case TableDataType.InAppositeWord:
				//tableCode = "1452065059&range=A4:S";
				break;
		}

		return $"{header}{tableCode}";
	}
	public void CharacterDataParse(string data)
	{
		characterDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 22 && 
			    int.TryParse(values[0], out int index) &&
			    int.TryParse(values[5], out int hp) &&
			    float.TryParse(values[6], out float hpIncrement) &&
			    int.TryParse(values[7], out int def) &&
			    float.TryParse(values[8], out float defIncrement) &&
			    int.TryParse(values[9], out int atk) &&
			    float.TryParse(values[10], out float atkIncrement) &&
			    int.TryParse(values[11], out int spd) &&
			    float.TryParse(values[12], out float spdIncrement) &&
			    int.TryParse(values[13], out int ats) &&
			    double.TryParse(values[14], out double atsIncrement) &&
			    int.TryParse(values[15], out int evd) &&
			    double.TryParse(values[16], out double evdIncrement) &&
				int.TryParse(values[17], out int crt) &&
			    double.TryParse(values[18], out double crtIncrement) &&
				int.TryParse(values[19], out int crtd) &&
			    double.TryParse(values[20], out double crtdIncrement)
				)
			{
				CharacterData characterData = new CharacterData();
	
				characterData.index         = index;
				characterData.name          = values[1];
				characterData.prefabName    = values[2];
				characterData.iconName      = values[3];
				characterData.attribute     = StringToStatusEnum(values[4]);
				characterData.hp            = hp;
				characterData.hpIncrement   = hpIncrement;
				characterData.def           = def;
				characterData.defIncrement  = defIncrement;
				characterData.atk           = atk;
				characterData.atkIncrement  = atkIncrement;
				characterData.spd           = spd;
				characterData.spdIncrement  = spdIncrement;
				characterData.ats           = ats;
				characterData.atsIncrement  = atsIncrement;
				characterData.evd           = evd;
				characterData.evdIncrement  = evdIncrement;
				characterData.crt           = crt;
				characterData.crtIncrement  = crtIncrement;
				characterData.crtd          = crtd;
				characterData.crtdIncrement = crtdIncrement;
				characterData.descript      = values[21];
				
				characterDB.Add(characterData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	public void MonsterDataParse(string data)
	{
		monsterDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 14 && 
			    int.TryParse(values[0], out int index) &&
			    int.TryParse(values[5], out int hp) &&
			    float.TryParse(values[6], out float hpIncrement) &&
			    int.TryParse(values[7], out int atk) &&
			    float.TryParse(values[8], out float atkIncrement) &&
			    int.TryParse(values[9], out int def) &&
			    float.TryParse(values[10], out float defIncrement) &&
			    int.TryParse(values[12], out int minGrade)
			   )
			{
				MonsterData monsterData = new MonsterData();
	
				monsterData.index         = index;
				monsterData.title         = values[1];
				monsterData.name          = values[2];
				monsterData.prefabName    = values[3];
				monsterData.iconName      = values[4];
				monsterData.hp            = hp;
				monsterData.hpIncrement   = hpIncrement;
				monsterData.atk           = atk;
				monsterData.atkIncrement  = atkIncrement;
				monsterData.def           = def;
				monsterData.defIncrement  = defIncrement;
				monsterData.attribute     = StringToStatusEnum(values[11]);
				monsterData.minGrade      = minGrade;
				monsterData.descript      = values[13];
				
				monsterDB.Add(monsterData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	public void IngredientItemDataParse(string data)
	{
		ingredientItemDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 5 && 
			    int.TryParse(values[0], out int index) &&
			    int.TryParse(values[2], out int grade)
			   )
			{
				IngredientItem ingredientItemData = new IngredientItem();
	
				ingredientItemData.index         = index;
				ingredientItemData.name          = values[1];
				ingredientItemData.grade         = grade;
				ingredientItemData.iconName      = values[3];
				ingredientItemData.descript      = values[4];

				ingredientItemDB.Add(ingredientItemData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	public void ConsumableItemDataParse(string data)
	{
		consumableItemDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 5 && 
			    int.TryParse(values[0], out int index) &&
			    int.TryParse(values[2], out int grade)
			   )
			{
				ConsumableItem consumableItemData = new ConsumableItem();
	
				consumableItemData.index         = index;
				consumableItemData.name          = values[1];
				consumableItemData.grade         = grade;
				consumableItemData.iconName      = values[3];
				consumableItemData.descript      = values[4];

				consumableItemDB.Add(consumableItemData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	// public void SelectEventDataParse(string data)
	// {
	// 	selectEventDB.Clear();
	// 	
	// 	string[] lines = data.Trim().Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
	// 	
	// 	foreach (string line in lines)
	// 	{
	// 		string[] values = line.Trim().Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
	// 		
	// 		if (values.Length == 4 && 
	// 		    int.TryParse(values[0], out int selectEventID) &&
	// 		    float.TryParse(values[2], out float selectEventStatusValue)
	// 		   )
	// 		{
	// 			SelectEventData selectEventData = new SelectEventData();
	//
	// 			selectEventData.selectEventID          = selectEventID;
	// 			selectEventData.selectEventStatus      = StringToStatusEnum(values[1]);
	// 			selectEventData.selectEventStatusValue = selectEventStatusValue;
	// 			selectEventData.selectEventDescription = values[3];
	// 			
	// 			selectEventDB.Add(selectEventData);
	// 		}
	// 		else
	// 		{
	// 			Debug.LogError($"Invalid data line: {line}");
	// 		}
	// 	}
	// }
	//
	// public void EndingDataParse(string data)
	// {
	// 	endingDB.Clear();
	// 	
	// 	string[] lines = data.Trim().Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
	// 	
	// 	foreach (string line in lines)
	// 	{
	// 		string[] values = line.Trim().Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
	// 		
	// 		if (values.Length == 19 && 
	// 		    int.TryParse(values[0], out int endingID) &&
	// 		    int.TryParse(values[3], out int endingOrder) &&
	// 		    int.TryParse(values[5], out int endingStatus1value) &&
	// 		    bool.TryParse(values[6], out bool endingStatus1valueCondition) &&
	// 		    int.TryParse(values[8], out int endingStatus2value) &&
	// 		    bool.TryParse(values[9], out bool endingStatus2valueCondition) &&
	// 		    int.TryParse(values[11], out int endingStatus3value) &&
	// 			bool.TryParse(values[12], out bool endingStatus3valueCondition) &&
	// 		    int.TryParse(values[14], out int endingStatus4value) &&
	// 		    bool.TryParse(values[15], out bool endingStatus4valueCondition) &&
	// 		    int.TryParse(values[17], out int endingGrade)
	// 		   )
	// 		{
	// 			EndingData endingData = new EndingData();
	//
	// 			endingData.endingID                    = endingID;
	// 			endingData.endingName                  = values[1];
	// 			endingData.endingImage                 = values[2];
	// 			endingData.endingOrder                 = endingOrder;
	// 			
	// 			endingData.endingStatus1               = StringToStatusEnum(values[4]);
	// 			endingData.endingStatus1value          = endingStatus1value;
	// 			endingData.endingStatus1valueCondition = endingStatus1valueCondition;
	// 			
	// 			endingData.endingStatus2               = StringToStatusEnum(values[7]);
	// 			endingData.endingStatus2value          = endingStatus2value;
	// 			endingData.endingStatus3valueCondition = endingStatus2valueCondition;
	// 			
	// 			endingData.endingStatus3               = StringToStatusEnum(values[10]);
	// 			endingData.endingStatus3value          = endingStatus3value;
	// 			endingData.endingStatus3valueCondition = endingStatus3valueCondition;
	// 			
	// 			endingData.endingStatus4               = StringToStatusEnum(values[13]);
	// 			endingData.endingStatus4value          = endingStatus4value;
	// 			endingData.endingStatus4valueCondition = endingStatus4valueCondition;
	//
	// 			endingData.endingDescription           = values[16];
	// 			endingData.endingGrade                 = endingGrade;
	// 			endingData.endingGradedescription      = values[18];
	// 			
	// 			endingDB.Add(endingData);
	// 		}
	// 		else
	// 		{
	// 			Debug.LogError($"Invalid data line: {line}");
	// 		}
	// 	}
	// }

	private Attribute StringToStatusEnum(string enumString)
	{
		Attribute statusType = (Attribute)Enum.Parse(typeof(Attribute), enumString);
			return statusType;
	}
	//
	// private CaseType StringToCaseTypeEnum(string enumString)
	// {
	// 	CaseType cardType = (CaseType)Enum.Parse(typeof(CaseType), enumString);
	//
	// 	return cardType;
	// }
}