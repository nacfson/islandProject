using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/Item/FishDataList")]
public class FishDataList : ScriptableObject
{
    public List<FishData> fishList = new List<FishData>();

    public FishData GetFishDataWithRarity()
    {
        FishData fish = null;
        int maxCnt = 0;
        foreach(ERarity rarity in Enum.GetValues(typeof(ERarity)))
        {
            int rare = (int)rarity;
            if(maxCnt < rare)
            {
                maxCnt = rare;
            }
        }   
        //1부터 Enum의 최댓값 중 랜덤으로 뽑음
        int rand = Random.Range(1,maxCnt + 1);

        if(fish == null)
        {
            Debug.LogError(String.Format("Can't Find it Fish: {0}",fish.ToString()));
            return null;
        }
        return fish;
    }
}
