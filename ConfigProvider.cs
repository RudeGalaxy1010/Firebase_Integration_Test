using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Extensions;
using System;

namespace IntegrationTest
{
    // Took it from
    // https://github.com/firebase/quickstart-unity/blob/master/remote_config/testapp/Assets/Firebase/Sample/RemoteConfig/UIHandler.cs
    //

    public class ConfigProvider
    {
        private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        private bool isFirebaseInitialized = false;
        private int _configNumber;

        public void Init()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                      "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }

        void InitializeFirebase()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();

            defaults.Add("config_test_string", "default local string");
            defaults.Add("config_test_int", 1);
            defaults.Add("config_test_float", 1.0);
            defaults.Add("config_test_bool", false);

            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
              .ContinueWithOnMainThread(task => {
                  isFirebaseInitialized = true;
              });
        }

        public int GetConfigNumber()
        {
            FetchDataAsync();
            return _configNumber;
        }

        public Task FetchDataAsync()
        {
            Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        void FetchComplete(Task fetchTask)
        {
            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task => { _configNumber = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("ConfigNumber").LongValue; });

                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    break;
            }
        }
    }
}
