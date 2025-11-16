using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class SelectCardCanvas : BaseCanvas
{
    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private Transform cardContainer;
    Dictionary<EntityConfigSO, Card> holderDict = new();

    public void Init(List<EntityID> selectableIds)
    {
        ClearCards();
        PopulateSelection(selectableIds);
    }

    public void OnReadyClick()
    {
        CameraManager.Instance.ToLawnView();
        UIManager.Instance.Open<GameplayCanvas>();
        UIManager.Instance.Close<SelectCardCanvas>();
        UIManager.Instance.GetCanvas<DeckCanvas>().ToGameplayPosition();
        LevelManager.Instance.StartLevel();
    }

    private void PopulateSelection(List<EntityID> selectableIds)
    {
        int count = selectableIds.Count;
        for (int i = 0; i < count; i++)
        {
            var config = GameDatabase.Instance.EntityDictionary[selectableIds[i]];
            var holder = cardFactory.CreateCard(config, cardContainer.transform);
            holder.SetSelectable(false);
            holderDict[config] = holder;
            cardFactory.CreateCard(config, holder.transform.position, holder.transform).SetSelectable(true);
        }
    }

    public void ReturnCard(Card card)
    {
        var holder = holderDict[card.EntityConfig];
        card.transform.SetParent(holder.transform);
        
        Tween.StopAll(card.transform);
        Tween.Position(card.transform, holderDict[card.EntityConfig].transform.position, 0.2f, ease: Ease.Linear);
    }

    public void ClearCards()
    {
        foreach(var card in holderDict.Values)
        {
            Destroy(card.gameObject);
        }
        holderDict.Clear();
    }
}
