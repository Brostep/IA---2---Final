using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image debtFree;
    public Image knife;
	public Image shovel;
	public Image flour;
    public Text moneyAmount;
    public Text seedAmount;
    public Text tomatoesAmount;


    public void UpdateUI(WorldModel wm)
    {
        moneyAmount.text = wm.money.ToString();
        seedAmount.text = wm.seeds.ToString();
		tomatoesAmount.text = wm.tomatoes.ToString();

		flour.enabled = wm.hasflour;
		knife.enabled = wm.item == 0;
		shovel.enabled = wm.item == 1;
		debtFree.enabled = wm.item == 2;
    }
}
