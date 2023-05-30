using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStatus : MonoBehaviour
{
    [SerializeField] private GameObject heroStatusPrefab;
    private Dictionary<Hero, HeroStatus> heroStatuses = new Dictionary<Hero, HeroStatus>();

    public void SetParty(Party party)
    {
        foreach (Hero h in party.heroes)
        {
            HeroStatus status = Instantiate(heroStatusPrefab, gameObject.transform).GetComponent<HeroStatus>();
            status.SetHero(h);
            heroStatuses.Add(h, status);
        }
    }

    public void RemoveAllHeroes()
    {
        foreach (Hero h in heroStatuses.Keys)
            Destroy(heroStatuses[h]);
        heroStatuses.Clear();
    }

    public void RemoveHero(Hero h)
    {
        Destroy(heroStatuses[h]);
        heroStatuses.Remove(h);
    }

    public void SetPartyHealth(Party party)
    {
        foreach (Hero h in party.heroes)
        {
            SetHeroHealth(h);
        }
    }

    public void SetHeroHealth(Hero h)
    {
        if (!heroStatuses.ContainsKey(h))
            return;

        heroStatuses[h].SetHeroHealth(h);
    }
}
