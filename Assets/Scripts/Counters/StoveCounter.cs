using System;
using UnityEngine;

public class StoveCounter : BaseCounter
{

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;

    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;

        switch (state)
        {
            case State.Idle:
                break;

            case State.Frying:
                fryingTimer += Time.deltaTime;

                if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                {
                    KitchenObjectSO outputSO = fryingRecipeSO.output; // store output
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outputSO, this);

                    state = State.Fried;
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
                break;

            case State.Fried:
                burningTimer += Time.deltaTime;

                if (burningRecipeSO != null && burningTimer >= burningRecipeSO.BurningTimerMax)
                {
                    KitchenObjectSO outputSO = burningRecipeSO.output; // store output
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outputSO, this);

                    state = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                }
                break;

            case State.Burned:
                // optional: can add visual effects or timers for burned state
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Stove empty
            if (player.HasKitchenObject())
            {
                KitchenObjectSO playerObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                if (HasRecipeWithInput(playerObjectSO))
                {
                    // Place object on stove
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(playerObjectSO);
                    fryingTimer = 0f;
                    state = State.Frying;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                }
                else
                {
                    Debug.Log("This item cannot be cooked on the stove.");
                }
            }
        }
        else
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            state = State.Idle;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO recipe in fryingRecipeSOArray)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO recipe in burningRecipeSOArray)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;
            }
        }
        return null;
    }
}