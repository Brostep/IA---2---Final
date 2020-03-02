using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Image debtFree;
    public Image knife;
	public Image shovel;
	public Image flour;
    public Text moneyAmount;
    public Text seedAmount;
    public Text tomatoesAmount;


    public void UpdateUI(WorldModel worldModel) {
        moneyAmount.text = worldModel.money.ToString();
        seedAmount.text = worldModel.seeds.ToString();
		tomatoesAmount.text = worldModel.tomatoes.ToString();

		flour.enabled = worldModel.hasflour;
		knife.enabled = worldModel.item == 0;
		shovel.enabled = worldModel.item == 1;
		debtFree.enabled = worldModel.item == 2;
    }
}
