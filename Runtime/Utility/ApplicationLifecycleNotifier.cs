using Zenject;
using System.Collections.Generic;
using UnityEngine;

namespace MyFw
{
    public interface IApplicationQuitReceiver
    {
        void OnApplicationQuit();
    }

    public interface IApplicationPauseReceiver
    {
        void OnApplicationPause(bool pauseStatus);
    }

    public class ApplicationLifecycleNotifier : MonoBehaviour
    {
        [Inject] private readonly List<IApplicationQuitReceiver> _quitReceivers;
        [Inject] private readonly List<IApplicationPauseReceiver> _pauseReceivers;
        
        private void OnApplicationQuit()
        {
            foreach (var receiver in _quitReceivers)
            {
                receiver.OnApplicationQuit();
            }
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            foreach (var receiver in _pauseReceivers)
            {
                receiver.OnApplicationPause(pauseStatus);
            }
        }
    }
}