using System;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
[CreateAssetMenu(menuName = "SO/Item/FishDataList")]
public class FishDataList : ScriptableObject
{
    public List<FishData> fishList = new List<FishData>();
    public FishData GetFishDataWithRarity(ItemRarityData itemRarityData)
    {
        FishData fish = null;

        //퍼센트로 설정해놓긴 했는데 가중치가 더 편할지도
        float randomPer = Random.Range(0,100f);
        float currentPer = 0f;
        ERarity targetRarity = ERarity.NONE;
        
        foreach(var r in itemRarityData.itemRarityList)
        {
            currentPer += r.percent;
            Debug.Log($"Current Per: {currentPer} rarityPercent: {r.percent} RandomPer: {randomPer}");
            if(randomPer <= currentPer)
            {
                targetRarity = r.rairty;
                break;
            }
        }

        if(targetRarity == ERarity.NONE) 
            Debug.LogError("TargetRarity is not defined correctly");

        List<FishData> curRarityFishList = new List<FishData>();
        curRarityFishList = (from f in fishList where f.rarity == targetRarity select f).ToList();
        //curRarityFishList = fishList.Where(f => f.rarity == targetRarity).ToList();

        int randIdx = Random.Range(0,curRarityFishList.Count);
        fish = curRarityFishList[randIdx];

        if(fish == null)
        {
            Debug.LogError(String.Format("Can't Find it Fish: {0}",fish.ToString()));
            return null;
        }
        return fish;
    }
}
