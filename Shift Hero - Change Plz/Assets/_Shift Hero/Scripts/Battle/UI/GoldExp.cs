using UnityEngine;
using UnityEngine.UI;

public class GoldExp : MonoBehaviour
{
    [SerializeField] Text _txtGold = null;
    [SerializeField] Text _txtExp = null;
    
    // Update is called once per frame
    void Update()
    {
        _txtGold.text = string.Format("{0:###,0}", BattleData.GetBattleGold());
        _txtExp.text = string.Format("{0:###,0}", BattleData.GetBattleExp());
    }
}
