using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Initial values
    [Header("Initial values:")]
	public int initialTomatoes;
	public int initialSeeds;
	public float initialMoney;
	public enum InitialItem { KNIFE, SHOVEL, DEBT_FREE, NOTHING }
	public InitialItem initialItem;
	public bool taxesPayed;
	public float speed;
	public bool givenGradmaAdvice;
    #endregion

    #region Money values
    [Header("Money values:")]
	public float pizzaPlacePrice;
	public float tomatoesSellPrice;
    public float seedsPrice;
    public float taxesPrice;
	public float wheatToFlourPrice; 
	public float salary;
    public float stealBounty;
	public float grandmaLoan;
    #endregion

    #region Costs
    [Header("Costs:")]
    public int plantTomatoesCost;
    public int getSeedsCost;
    public int govtAgreementCost;
    public int wheatToFlourCost;
    public int payTaxesCost;
    public int sellTomatoesCost;
    public int workCost;
    public int stealCost;
	public int grandmaCost;
    #endregion

    #region Spots
    [Header("Spots:")]
    public Transform sellTomatoesSpot;
    public Transform pizzaPlaceSpot;
    public Transform farmSpot;
    public Transform getSeedsSpot;
    public Transform payTaxesSpot;
    public Transform capitolSpot;
    public Transform workSpot;
    public Transform wheatToFlourHouse;
	public Transform grandmaHouseSpot;
	public Transform[] stealSpot;
    #endregion
    
    [Header("Side Logic references")]
    public TomatoesController tomatoesController;
    public GameObject macri;
    public ParticleSystem moneyPS;
    public ParticleSystem tomatoePS;
    public ParticleSystem seedPS;
    public ParticleSystem flourPS;
    public ParticleSystem debtFreePS;
	public ParticleSystem knifePS;
	public ParticleSystem shovelPS;
	public LabController lab;

	private Queue<Tuple<string,WorldModel>> _actionQueue;
    private Walker _walker;
    private UIController _ui;
    private int _stealCount;

    private void Start() {
        _ui = GetComponent<UIController>();
        _walker = GetComponent<Walker>();
        _walker.MovementSpeed = speed;
        macri.SetActive(false);
        Plan();
    }

    private void Plan() {
        var initialModel = new WorldModel();
        initialModel.tomatoes = initialTomatoes;
        initialModel.seeds = initialSeeds;
        initialModel.money = initialMoney;
		initialModel.item = (int) initialItem;
		initialModel.givenAdvice = givenGradmaAdvice;

		if (initialModel.item == (int)InitialItem.DEBT_FREE)
			initialModel.hasPayedTaxes = true;
		else
			initialModel.hasPayedTaxes = taxesPayed;
        
		_ui.UpdateUI(initialModel);
		CostCorrection(initialModel.item);
        Func<WorldModel, bool> goal = (g) => g.isPizzaMaster;
        var actions = GetActions();
        var initialState = new GoapState(actions, null, initialModel, goal, Heuristic);
        var plan = new GOAP(initialState).Execute();
		_actionQueue = new Queue<Tuple<string, WorldModel>>();

		foreach (var step in plan) {
			Debug.Log("Action: " + step.Item1);

            if (!step.Item1)
                continue;

            var path = step.Item3.Reverse();

            foreach (var state in path) {
                if (state.GeneratedAction != null) {
					Debug.Log("No action left to do.");
					_actionQueue.Enqueue(Tuple.Create(state.GeneratedAction.Name, state.CurrentWorldModel));
                }
            }
        }

        if (_actionQueue.Count == 0) 
            Debug.Log("No action left to do.");
        else
            ExecuteAction();
    }

    private void ExecuteAction() {
		Debug.Log("ExecuteAction: ");

		if (_actionQueue.Count > 0) {
            var dequeued = _actionQueue.Dequeue();
            Debug.Log("Action: " + dequeued.Item1);
            StartCoroutine(dequeued.Item1, dequeued.Item2);
        }

        else
            Plan();
    }

    public void Stop() {
        StopAllCoroutines();

        if(_walker)
            _walker.Stop();
    }

    private void CostCorrection(int item) {
		if (item == (int)InitialItem.NOTHING)
			grandmaCost = 1;
		else if (item == (int)InitialItem.SHOVEL)
			workCost = 1;
		else if (item == (int)InitialItem.KNIFE) {
			grandmaCost = 20;
			stealCost = 1;
		}
		else if (item == (int)InitialItem.DEBT_FREE) {
			grandmaCost = 20;
			govtAgreementCost = 1;
		}
	}

    private List<GoapAction> GetActions() {
        var listOfActions = new List<GoapAction>() {
            new GoapAction(PlayerActionKey.GetPizzaPlace)
                .AddPrecondition(x => x.hasPayedTaxes == true)
				.AddPrecondition(x => x.item ==  (int) InitialItem.DEBT_FREE)
				.AddPrecondition(x => x.money >= pizzaPlacePrice)
				.AddPrecondition(x => x.hasflour == true)
				.AddEffect(x => {
                    var wm = new WorldModel();
                    wm.isPizzaMaster = true;
                    return wm;
                })
                .AddCost(0),

			 new GoapAction(PlayerActionKey.GoToGrandmaHouse)
				.AddPrecondition(x => x.item == (int) InitialItem.NOTHING)
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.money += grandmaLoan;
					return wm;
				})
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					var itemR = UnityEngine.Random.Range(0,10);

                    if (itemR > 5)
						wm.item = (int)InitialItem.KNIFE;
					else
						wm.item = (int)InitialItem.SHOVEL;
					
					return wm;
				})
				.AddCost(grandmaCost),

			 new GoapAction(PlayerActionKey.GetSeed)
				.AddPrecondition(x => x.money > seedsPrice)
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.seeds ++;
					return wm;
				})
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.money -= seedsPrice;
					return wm;
				})
				.AddCost(getSeedsCost),

			new GoapAction(PlayerActionKey.PlantTomatoes)
				.AddPrecondition(x => x.item == (int) InitialItem.SHOVEL)
				.AddPrecondition(x => x.seeds > 0)
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.seeds --;
					return wm;
				})
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.tomatoes += 3;
					return wm;
				})
				.AddCost(plantTomatoesCost),

			new GoapAction(PlayerActionKey.SellTomatoes)
				.AddPrecondition(x => x.tomatoes >= 3)
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.money += tomatoesSellPrice;
					return wm;
				}).
				AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.tomatoes -= 3;
					return wm;
				})
				.AddCost(sellTomatoesCost),

            new GoapAction(PlayerActionKey.Steal)
                .AddPrecondition(x => x.item == (int) InitialItem.KNIFE)
				.AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.money += stealBounty;
                    return wm;
                })
                .AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.stealCount ++;
                    return wm;
                })  
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.isAThief = true;
					return wm;
				})
				.AddCost(stealCost),

			new GoapAction(PlayerActionKey.PayTaxes)
                .AddPrecondition(x => x.money >= taxesPrice)
                .AddPrecondition(x => x.item != (int) InitialItem.DEBT_FREE)
                .AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.money -= taxesPrice;
                    return wm;
                })
                .AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.item = (int) InitialItem.DEBT_FREE;
                    return wm;
                })
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.hasPayedTaxes = true;
					return wm;
				})
				.AddCost(payTaxesCost),

            new GoapAction(PlayerActionKey.DealGob)
				.AddPrecondition(x => x.hasPayedTaxes == true)
                .AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.hasPayedTaxes = true;
                    return wm;
                })
                .AddCost(govtAgreementCost),

            new GoapAction(PlayerActionKey.Work)
				.AddPrecondition(x => x.isAThief == false)
				.AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.money += salary;
                    return wm;
                })
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.stealCount ++;
					return wm;
				})
				.AddCost(govtAgreementCost),

            new GoapAction(PlayerActionKey.WheatToFluor)
				.AddPrecondition(x => x.money >= wheatToFlourPrice)
				.AddPrecondition(x => x.item ==  (int) InitialItem.DEBT_FREE)
				.AddEffect(x => {
                    var wm = WorldModel.Clone(x);
                    wm.money -= wheatToFlourPrice;
                    return wm;
                })
				.AddEffect(x => {
					var wm = WorldModel.Clone(x);
					wm.hasflour = true;
					return wm;
				})
				.AddCost(wheatToFlourCost)
        };

        return listOfActions;
    }

    private float Heuristic(WorldModel next, WorldModel current) {
        var result = 10f;

        //El objetivo basicamente es generar suficiente plata como para conseguir el laboratorio
        //El acuerdo se consigue con plata, si bien sea comprando un arma para secuestrar a la hija del presidente y conseguir el acuerdo, o comprando el acuerdo directamente, por eso se traduce a plata
        result += current.hasPayedTaxes ? 0 : govtAgreementCost / 100f;
        result -= result - current.money / 100f > 0 ? (result - current.money) / 100f : result;

		//Esta variable esta agregada para que no se la pase robando
		result += current.stealCount;

		if (next.isPizzaMaster)
			result = 0f;

		return result;
    }

    #region Coroutines
    private IEnumerator GetPizzaPlace(WorldModel worldModel) {
        _walker.SetDestination(pizzaPlaceSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);
        _ui.UpdateUI(worldModel);
        lab.SellLab();
        GameManager.Instance.WinGame(gameObject.name);
    }

    private IEnumerator GoToGrandmaHouse(WorldModel worldModel) {
		_walker.SetDestination(grandmaHouseSpot.position);

		while (!_walker.ReachDestionation)
			yield return null;
		
        moneyPS.Play();

        yield return new WaitForSeconds(2f);

        switch (worldModel.item) {
			case 0:
				knifePS.Play();
				break;

			case 1:
				shovelPS.Play();
				break;

            default:
                throw new ArgumentNullException(worldModel.item.ToString());
		}

		_ui.UpdateUI(worldModel);
        ExecuteAction();
	}

    private IEnumerator Steal(WorldModel worldModel) {
		_walker.SetDestination(stealSpot[_stealCount].position);

		while (!_walker.ReachDestionation)	
			yield return null;
	
		_stealCount++;

		if (_stealCount > 1)
			_stealCount = 0;
		
		moneyPS.Play();

		yield return new WaitForSeconds(2f);

        _ui.UpdateUI(worldModel);
		ExecuteAction();
	}

    private IEnumerator PlantTomatoes(WorldModel worldModel) {
       _walker.SetDestination(farmSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;


        tomatoesController.Grow();

        yield return new WaitForSeconds(1.2f);

        tomatoePS.Play();

        yield return new WaitForSeconds(0.5f);

        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }

    private IEnumerator SellTomatoes(WorldModel worldModel) {
        _walker.SetDestination(sellTomatoesSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        moneyPS.Play();

        yield return new WaitForSeconds(2f);
        
        //TODO: Sell tomatoes
        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }

    private IEnumerator GetSeeds(WorldModel worldModel) {
        _walker.SetDestination(getSeedsSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        seedPS.Play();

        yield return new WaitForSeconds(1f);
        
        //TODO: Buy Seeds
        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }

    private IEnumerator PayTaxes(WorldModel worldModel) {
        _walker.SetDestination(payTaxesSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        yield return new WaitForSeconds(1f);

        debtFreePS.Play();
        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }

    private IEnumerator DealWithGovt(WorldModel worldModel) {
        _walker.SetDestination(capitolSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        flourPS.Play();

        yield return new WaitForSeconds(2f);
        
        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }

    private IEnumerator Work(WorldModel worldModel) {
        _walker.SetDestination(workSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        while (true) {
            transform.forward = Vector3.Slerp(transform.forward, workSpot.forward, Time.deltaTime * 10);

            if (Math.Abs(Vector3.Magnitude(transform.forward - workSpot.forward)) < 0.1) {
                transform.forward = workSpot.forward;
                break;
            }

            yield return null;
        }

        moneyPS.Play();

        yield return new WaitForSeconds(2f);
        
        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }

    private IEnumerator WheatToFluor(WorldModel worldModel) {
        _walker.SetDestination(capitolSpot.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);

        macri.SetActive(true);
        _walker.SetDestination(wheatToFlourHouse.position);

        while (!_walker.ReachDestionation)
            yield return null;
        
        macri.SetActive(false);
        moneyPS.Play();
        flourPS.Play();

        yield return new WaitForSeconds(2f);
        
        _ui.UpdateUI(worldModel);
        ExecuteAction();
    }
    #endregion
}
