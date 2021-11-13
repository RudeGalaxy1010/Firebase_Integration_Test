using System.Linq;
using UnityEngine;

namespace IntegrationTest
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private MatchSettings _matchSettings;
        [SerializeField] private GameObject _testCube;

        private ConfigProvider _configProvider;
        private int _configNumber;

        private void Start()
        {
            _configProvider = new ConfigProvider();
            _configProvider.Init();
        }

        public void ApplyConfig()
        {
            _configNumber = _configProvider.GetConfigNumber();
            Match match = _matchSettings.Matches.First(m => m.ConfigNumber == _configNumber);
            _testCube.GetComponent<Renderer>().material = match.Material;
        }
    }
}
