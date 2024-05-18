﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class StoveCounter : BaseCounter
{

    [SerializeField] private FryingRecipeListSO fryingRecipeList;
    [SerializeField] private FryingRecipeListSO burningRecipeList;
    [SerializeField] private StoveCounterVisual stoveCounterVisual;
    [SerializeField] private ProgressBarUI progressBarUI;

    public enum StoveState
    {
        Idle,
        Frying,
        Burning
    }

    private FryingRecipe fryingRecipe;
    private float fryingTimer = 0;
    private StoveState state = StoveState.Idle;

    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject())
        {//Tenha ingredientes em mãos
            if (IsHaveKitchenObject() == false  )
            {//Atualmente não há nada no balcão

                if(fryingRecipeList.TryGetFryingRecipe(player.GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe fryingRecipe))
                {
                    TransferKitchenObject(player, this);
                    StartFrying(fryingRecipe);
                }
                else if(burningRecipeList.TryGetFryingRecipe(player.GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe burningRecipe))
                {
                    TransferKitchenObject(player, this);
                    StartBurning(burningRecipe);
                }
                else
                {

                }


            }
            else
            {//Atualmente ja tem coisa no balcão

            }
        }
        else
        {//Nao tenha
            if (IsHaveKitchenObject() == false)
            {//Atualmente não há nada no balcão

            }
            else
            {//Atualmente ja tem coisa no balcão
                TurnToIdle();
                TransferKitchenObject(this, player);
            }
        }
    }

    private void Update()
    {
        switch(state)
        {
            case StoveState.Idle:
                break;
            case StoveState.Frying:
                fryingTimer += Time.deltaTime;
                progressBarUI.UpdateProgress(fryingTimer/ fryingRecipe.fryingTime);
                if( fryingTimer >= fryingRecipe.fryingTime )
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(fryingRecipe.output.prefab);
                    

                    burningRecipeList.TryGetFryingRecipe(GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe newfryingRecipe);
                    StartBurning(newfryingRecipe);
                }
                break;
            case StoveState.Burning:
                fryingTimer += Time.deltaTime;
                progressBarUI.UpdateProgress(fryingTimer / fryingRecipe.fryingTime);
                if (fryingTimer >= fryingRecipe.fryingTime)
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(fryingRecipe.output.prefab);
                    TurnToIdle();
                }
                break;
            default:
                break;

        }
    }

    private void StartFrying(FryingRecipe fryingRecipe)
    {
        fryingTimer = 0;
        this.fryingRecipe = fryingRecipe;
        state = StoveState.Frying;
        stoveCounterVisual.ShowStoveEffect();
    }
    private void StartBurning(FryingRecipe fryingRecipe)
    {
        if(fryingRecipe == null )
        {
            Debug.LogWarning("Não foi possível obter a receita da Queima, não foi possível realizar a Queima.");
            TurnToIdle();
            return;
        }
        stoveCounterVisual.ShowStoveEffect();
        fryingTimer = 0;
        this.fryingRecipe = fryingRecipe;
        state = StoveState.Burning;
    }
    private void TurnToIdle()
    {
        progressBarUI.Hide();
        state = StoveState.Idle;
        stoveCounterVisual.HideStoveEffect();
    }

}
