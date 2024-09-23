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
	public List<WeaponCraftData>    weaponCraftDB;
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
					WeaponDataParse(data);
					break;
				case TableDataType.WeaponCraft:
					WeaponCraftDataParse(data);
					break;
				case TableDataType.Armor:
					//SelectEventDataParse(data);
					break;
				case TableDataType.AllEvent:
					AllEventDataParse(data);
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
				tableCode = "943329813&range=A4:O";
				break;
			case TableDataType.WeaponCraft:
				tableCode = "454813452&range=A4:Q";
				break;
			case TableDataType.Armor:
				//tableCode = "270773085&range=A4:D";
				break;
			case TableDataType.ArmorCraft:
				//tableCode = "";
			case TableDataType.AllEvent:
				tableCode = "1260771282&range=A4:D";
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
				characterData.attribute     = StringToAttributeEnum(values[4]);
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
				monsterData.attribute     = StringToAttributeEnum(values[11]);
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
	public void WeaponDataParse(string data)
	{
		weaponDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 15 && 
			    int.TryParse(values[0], out int index) &&
			    int.TryParse(values[10], out int atk) &&
			    int.TryParse(values[11], out int ap) &&
			    double.TryParse(values[12], out double crt) &&
			    float.TryParse(values[13], out float bornGrade) &&
			    int.TryParse(values[14], out int skillIndex)
			   )
			{
				WeaponData weaponData = new WeaponData();
	
				weaponData.index         = index;
				weaponData.name          = values[1];
				weaponData.weaponType    = StringToWeaponTypeEnum(values[2]);
				weaponData.attribute     = StringToAttributeEnum(values[3]);
				weaponData.handRPrefab   = values[4];
				weaponData.handLPrefab   = values[5];
				weaponData.attackEffect  = values[6];
				weaponData.projectile    = values[7];
				weaponData.skilleffect   = values[8];
				weaponData.iconName      = values[9];
				weaponData.atk           = atk;
				weaponData.ap            = ap;
				weaponData.crt           = crt;
				weaponData.bornGrade     = bornGrade;
				weaponData.skillIndex    = skillIndex;

				weaponDB.Add(weaponData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	
	public void WeaponCraftDataParse(string data)
	{
		weaponCraftDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 17 && 
			    int.TryParse(values[0], out int index) &&
			    int.TryParse(values[2], out int weaponIndex) &&
			    float.TryParse(values[3], out float grade) &&
			    int.TryParse(values[4], out int ingre1) &&
			    int.TryParse(values[5], out int ingre1Amount) &&
			    int.TryParse(values[6], out int ingre2) &&
			    int.TryParse(values[7], out int ingre2Amount) &&
			    int.TryParse(values[8], out int ingre3) &&
			    int.TryParse(values[9], out int ingre3Amount) &&
			    int.TryParse(values[10], out int ingre4) &&
			    int.TryParse(values[11], out int ingre4Amount) &&
			    int.TryParse(values[12], out int ingre5) &&
			    int.TryParse(values[13], out int ingre5Amount) &&
			    int.TryParse(values[14], out int ingre6) &&
			    int.TryParse(values[15], out int ingre6Amount) &&
			    int.TryParse(values[16], out int gold)
			   )
			{
				WeaponCraftData weaponCraftData = new WeaponCraftData();
	
				weaponCraftData.index         = index;
				weaponCraftData.name          = values[1];
				weaponCraftData.weaponIndex   = weaponIndex;
				weaponCraftData.grade         = grade;
				weaponCraftData.ingre1        = ingre1;
				weaponCraftData.ingre1Amount  = ingre1Amount;
				weaponCraftData.ingre2        = ingre2;
				weaponCraftData.ingre2Amount  = ingre2Amount;
				weaponCraftData.ingre3        = ingre3;
				weaponCraftData.ingre3Amount  = ingre3Amount;
				weaponCraftData.ingre4        = ingre4;
				weaponCraftData.ingre4Amount  = ingre4Amount;
				weaponCraftData.ingre5        = ingre5;
				weaponCraftData.ingre5Amount  = ingre5Amount;
				weaponCraftData.ingre6        = ingre6;
				weaponCraftData.ingre6Amount  = ingre6Amount;
				weaponCraftData.gold          = gold;

				weaponCraftDB.Add(weaponCraftData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	
	public void AllEventDataParse(string data)
	{
		allEventDB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 4 && 
			    int.TryParse(values[0], out int index) &&
			    bool.TryParse(values[2], out bool isUsing) &&
			    double.TryParse(values[3], out double eventRate)
			   )
			{
				AllEventData allEventData = new AllEventData();
	
				allEventData.index         = index;
				allEventData.name          = values[1];
				allEventData.isUsing       = isUsing;
				allEventData.eventRate     = eventRate;

				allEventDB.Add(allEventData);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}
	
	public void Event1DataParse(string data)
	{
		event1DB.Clear();
		
		string[] lines = data.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		
		foreach (string line in lines)
		{
			string[] values = line.Trim().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (values.Length == 5 && 
			    int.TryParse(values[0], out int index) &&
			    bool.TryParse(values[2], out bool isUsing) &&
			    int.TryParse(values[3], out int goldAmount) &&
			    double.TryParse(values[4], out double eventRate)
			   )
			{
				Event1Data event1Data = new Event1Data();
	
				event1Data.index         = index;
				event1Data.name          = values[1];
				event1Data.isUsing       = isUsing;
				event1Data.goldAmount    = goldAmount;
				event1Data.eventRate     = eventRate;

				event1DB.Add(event1Data);
			}
			else
			{
				Debug.LogError($"Invalid data line: {line}");
			}
		}
	}

	private Attribute StringToAttributeEnum(string enumString)
	{
		Attribute attributeType = (Attribute)Enum.Parse(typeof(Attribute), enumString);
		return attributeType;
	}
	
	private WeaponType StringToWeaponTypeEnum(string enumString)
	{
		WeaponType weaponType = (WeaponType)Enum.Parse(typeof(WeaponType), enumString);
		return weaponType;
	}
}