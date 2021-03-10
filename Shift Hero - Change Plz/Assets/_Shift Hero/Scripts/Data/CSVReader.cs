using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class CSVReader : MonoBehaviour
{
    public static bool isSetting = false;

    static readonly string itemPath = "Data/Item";                       // 아이템 DB 경로
    static readonly string playerDBPath = "Data/PlayerData";             // Player DB 경로
    static readonly string monsterDBPath = "Data/MonsterData";           // Monster DB 경로
    static readonly string shopPath = "Data/ShopData";                   // 상점 DB 경로
    static readonly string achievePath = "Data/AchieveData";             // 업적 DB 경로
    static readonly string achieveDetailPath = "Data/AchieveDetailData"; // 업적 디테일 DB 경로
    static readonly string perkPath = "Data/PerkData";                   // 퍽 DB 경로
    static readonly string stagePath = "Data/StageData";                 // 스테이지 DB 경로
    static readonly string levelTablePath = "Data/PlayerLevelUpTable";         // 스테이지 DB 경로
    static readonly string accountLevelTablePath = "Data/AccountLevelUpTable"; // 스테이지 DB 경로
    static readonly string dialoguePath = "Data/DialogueData";  // 대사 테이블 경로
    static readonly string eventPath = "Data/EventData";        // 대사 이벤트 테이블 경로



    // 상점 구분자
    const string WEAPON = "10", ARMOR = "11", RUNE = "20", CHARM = "30", ACCOUNT = "40";

    // CSV Split
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    private void Awake()
    {
        if (!isSetting)
        {
            isSetting = true;

            ItemDBSetting();                // 아이템 DB 세팅
            ShopDBSetting();                // 상점 DB 세팅
            PerkDBSeeting();                // 스킬 DB 세팅
            StageDBSetting();               // 스테이지 DB 세팅
            DialogueDBSetting();            // 대사 이벤트 관련 DB 세팅

            PlayerDBSetting();              // 캐릭터 데이터 세팅
            MonsterDBSetting();             // 몬스터 데이터 세팅
            SettingAchieve();               // 업적 세팅
            SettingAchieveDetail();         // 업적별 세부 목표 세팅

            SettingLevelUpTable();          // 레밸업 테이블 세팅
            SettingAccountLevelUpTable();   // 계정 레밸업 테이블 세팅
            BonusAbility.LoadAbility();
        }
    }

    

    void ItemDBSetting()
    {
        TextAsset data = Resources.Load(itemPath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1) return;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            int id = int.Parse(values[0]);
            string name = values[1];
            bool isStockable = bool.Parse(values[2]);
            bool isCash = bool.Parse(values[3]);
            int price = int.Parse(values[4]);
            string desc = values[5];
            string optionDesc = values[6];
            Sprite sprite = SpriteDB.GetItemSprite(id);
            string optionType = values[7].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
            string optionNum = values[8].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
            string[] optionTypes = Regex.Split(optionType, ",");
            string[] optionNums = Regex.Split(optionNum, ",");
            List<ItemOption> optionList = new List<ItemOption>();
            if (optionTypes.Length >= 1)
            {
                for (int k = 0; k < optionTypes.Length; k++)
                {
                    ItemOption option = new ItemOption()
                    {
                        optionType = optionTypes[k],
                        num = float.Parse(optionNums[k])
                    };
                    optionList.Add(option);
                }
            }

            Item item = new Item(id, name, isStockable, isCash, price, desc, optionDesc, sprite, optionList);
            ItemDB.AddItem(item.id, item);
        }
    }





    void PlayerDBSetting()
    {
        TextAsset data = Resources.Load(playerDBPath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            int id = int.Parse(values[0].ToString());
            string name = values[1];
            string job = values[2];
            string desc = values[3];
            int hp = int.Parse(values[4].ToString());
            Sprite sprite = SpriteDB.GetPlayerSprite(id);
            int def = int.Parse(values[5].ToString());
            int atk = int.Parse(values[6].ToString());
            float atkSpd = float.Parse(values[7].ToString());
            float atkRange = float.Parse(values[8].ToString());
            float criRate = float.Parse(values[9].ToString());
            float criMtp = float.Parse(values[10].ToString());
            float avdRate = float.Parse(values[11].ToString());
            float moveSpd = float.Parse(values[12].ToString());
            int recoverHp = int.Parse(values[13].ToString());
            float recoverTime = float.Parse(values[14].ToString());
            int lvUpHp = int.Parse(values[15].ToString());
            int lvUpAtk = int.Parse(values[16].ToString());
            int lvUpDef = int.Parse(values[17].ToString());
            int price = int.Parse(values[18].ToString());

            PlayerData pData = new PlayerData()
            {
                id = id,
                isOpen = false,
                name = name,
                job = job,
                desc = desc,
                price = price,
                hp = hp,
                level = 1,
                exp = 0,
                sprite = sprite,
                def = def,
                atk = atk,
                attackSpeed = atkSpd,
                attackRange = atkRange,
                criRate = criRate,
                criMultiply = criMtp,
                avoidanceRate = avdRate,
                moveSpeed = moveSpd,
                recoverHp = recoverHp,
                recoverTime = recoverTime,
                levelupHp = lvUpHp,
                levelupAtk = lvUpAtk,
                levelupDef = lvUpDef
            };

            PlayerDB.AddItem(pData.id, pData);
        }
    }

    private void MonsterDBSetting()
    {
        TextAsset data = Resources.Load(monsterDBPath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            int id = int.Parse(values[0].ToString());
            Sprite sprite = SpriteDB.GetMonsterSprite(id);
            string name = values[1];
            string desc = values[2];
            int exp = int.Parse(values[3].ToString());
            int gold = int.Parse(values[4].ToString());
            int hp = int.Parse(values[5].ToString());
            int def = int.Parse(values[6].ToString());
            int atk = int.Parse(values[7].ToString());
            float atkSpd = float.Parse(values[8].ToString());
            float atkRange = float.Parse(values[9].ToString());
            float viewRange = float.Parse(values[10].ToString());
            float criRate = float.Parse(values[11].ToString());
            float criMtp = float.Parse(values[12].ToString());
            float avdRate = float.Parse(values[13].ToString());
            float moveSpd = float.Parse(values[14].ToString());
            int recoverHp = int.Parse(values[15].ToString());
            float recoverTime = float.Parse(values[16].ToString());
            string animPath = values[17];

            MonsterData pData = new MonsterData(id, sprite, name, desc, gold, exp, hp, def, 
                atk, atkSpd, atkRange, viewRange, criRate, criMtp, avdRate, moveSpd, recoverHp, recoverTime, animPath);

            MonsterDB.AddMonster(pData.id, pData);
        }
    }


    private void ShopDBSetting()
    {
        TextAsset data = Resources.Load(shopPath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        List<int>[] shopList = new List<int>[5];
        for(int i = 0; i < shopList.Length; i++)
        {
            shopList[i] = new List<int>();
        }

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            int id = int.Parse(values[0].ToString());
            string type = id.ToString().Substring(0, 2);

            switch (type)
            {
                case WEAPON: shopList[0].Add(id); break;
                case ARMOR: shopList[1].Add(id); break;
                case RUNE: shopList[2].Add(id); break;
                case CHARM: shopList[3].Add(id); break;
                case ACCOUNT: shopList[4].Add(id); break;
            }
        }

        ShopDB.SetShopItem(shopList);
    }

    void DialogueDBSetting()
    {
        // 대사 추가
        TextAsset data = Resources.Load(dialoguePath) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            Dialogue dialogue = new Dialogue
            {
                id = int.Parse(values[0]),
                tag = values[1],
                characterID = int.Parse(values[2]),
                line = values[3],
            };
            DialogueDB.AddDialogue(dialogue);
        }

        // 이벤트 추가
        data = Resources.Load(eventPath) as TextAsset;
        lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            DialogueEvent dialogueEvent = new DialogueEvent
            {
                id = int.Parse(values[0]),
                condition = new Condition()
                {
                    type = int.Parse(values[1]),
                    option = int.Parse(values[2])
                },
                isShow = false,
                lineStartID = int.Parse(values[3]),
                lineEndID = int.Parse(values[4]),
            };
            DialogueDB.AddDialogueEvent(dialogueEvent);
        }


        DialogueDB.LoadEventData();
    }


    void SettingAchieve()
    {
        TextAsset data = Resources.Load(achievePath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
   
            int id = int.Parse(values[0]);
            Sprite sprite = SpriteDB.GetAchieveSprite(i - 1);
            string title = values[1];
            string desc = values[2];
            Achieve achieve = new Achieve(id, sprite, title, desc, 0);
            
            AchieveDB.AddAchieve(achieve.id, achieve);
        }

        AchieveDB.LoadAchieveData();

    }

    void SettingAchieveDetail()
    {
        TextAsset data = Resources.Load(achieveDetailPath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;


        int currentID = 0;
        int loopBack = 0;

        for (var i = 1; i < lines.Length; i++)
        {
            List<AchieveDetail> achieveDetailList = new List<AchieveDetail>();

            int num = 0;

            for (var k = i; k < lines.Length; k++)
            {
                loopBack++;

                if (loopBack > 200)
                    return;

                AchieveDetail detail = new AchieveDetail();

                var values = Regex.Split(lines[k], SPLIT_RE);

                
                int refId = int.Parse(values[0]);

                if(currentID != refId)
                {
                    if (num == 0)
                        return;

                    i = k - 1;
                    break;
                }

                int count = int.Parse(values[1]);

                string rewardType = values[2].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
                string rewardCount = values[3].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
                string[] rewardTypes = Regex.Split(rewardType, ",");
                string[] rewardCounts = Regex.Split(rewardCount, ",");

                List<AchieveReward> rewardList = new List<AchieveReward>();
                if (rewardTypes.Length >= 1)
                {
                    for (int l = 0; l < rewardTypes.Length; l++)
                    {
                        AchieveReward reward = new AchieveReward()
                        {
                            rewardID = int.Parse(rewardTypes[l]),
                            rewardCount = int.Parse(rewardCounts[l]),
                        };
                        rewardList.Add(reward);
                    }
                }

                detail.num = num;
                detail.count = count;
                detail.rewards = rewardList;

                //Debug.Log($"{currentID} , {num}, {count}");


                achieveDetailList.Add(detail);
                num++;
            }

            AchieveDB.AddAchieveDetail(currentID, achieveDetailList);
            currentID++;
        }
    }


    private void PerkDBSeeting()
    {
        TextAsset data = Resources.Load(perkPath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        PerkMenu perkMenu = FindObjectOfType<PerkMenu>();
        PerkInfo[] perkInfo = new PerkInfo[lines.Length - 1];

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            int id = int.Parse(values[0]);
            int needID = int.Parse(values[1]);
            Sprite sprite = SpriteDB.GetPerkSprite(i - 1);
            string name = values[2];
            string desc = values[3];
            int sp = int.Parse(values[4]);
            string option = values[5];
            float optionNum = float.Parse(values[6]);
            perkInfo[i - 1] = new PerkInfo(needID, id, name, desc, sprite, sp, false, option, optionNum);
        }

        perkMenu.SetPerk(perkInfo);

    }


    private void StageDBSetting()
    {
        TextAsset data = Resources.Load(stagePath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            int id = int.Parse(values[0]);

            List<StageMonster> monsterList = new List<StageMonster>();
            string[] monsterIDs = Regex.Split(values[1].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS), ",");
            string[] monsterCounts = Regex.Split(values[2].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS), ",");
            string[] monsterLocations = Regex.Split(values[3].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS), ",");

            if (monsterIDs.Length >= 1)
            {
                for (int l = 0; l < monsterIDs.Length; l++)
                {
                    StageMonster monster = new StageMonster
                    {
                        id = int.Parse(monsterIDs[l]),
                        count = int.Parse(monsterCounts[l]),
                        location = monsterLocations[l].ToString()
                    };

                    monsterList.Add(monster);
                }
            }


            List<StageClearReward> rewardList = new List<StageClearReward>();
            string[] rewardIDs = Regex.Split(values[4].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS), ",");
            string[] rewardCounts = Regex.Split(values[5].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS), ",");

            if (rewardIDs.Length >= 1)
            {
                for (int l = 0; l < rewardIDs.Length; l++)
                {
                    StageClearReward reward = new StageClearReward
                    {
                        id = int.Parse(rewardIDs[l]),
                        count = int.Parse(rewardCounts[l]),
                    };

                    rewardList.Add(reward);
                }
            }

            Stage stage = new Stage
            {
                stageID = id,
                stageMonsterList = monsterList,
                stageClearRewardList = rewardList
            };
            StageDB.StageAdd(stage);

        }
    }

    void SettingLevelUpTable()
    {
        TextAsset data = Resources.Load(levelTablePath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        int[] levelUpTable = new int[lines.Length - 1];

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            levelUpTable[i - 1] = int.Parse(values[1]);
        }

        PlayerDB.SetLevelUpTable(levelUpTable);
    }
    void SettingAccountLevelUpTable()
    {
        TextAsset data = Resources.Load(accountLevelTablePath) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return;

        int[] levelUpTable = new int[lines.Length - 1];

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            levelUpTable[i - 1] = int.Parse(values[1]);
        }

        PlayerDB.SetAccountLevelUpTable(levelUpTable);
    }


}
