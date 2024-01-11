using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartyLayout", menuName = "Other/New Party Layout")]
public class PartyLayout : ScriptableObject
{
    [SerializeField] private List<PartyCandidate> candidates = new List<PartyCandidate>();

    public List<PartyCandidate> GetPartyCandidates() {return candidates;}
}
