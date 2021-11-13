using System.Collections.Generic;
using UnityEngine;

namespace IntegrationTest
{
    [CreateAssetMenu(fileName = "New Match Settings", menuName = "Custom/Match Settings")]
    public class MatchSettings : ScriptableObject
    {
        public List<Match> Matches;
    }

    [System.Serializable]
    public struct Match
    {
        public int ConfigNumber;
        public Material Material;
    }
}
