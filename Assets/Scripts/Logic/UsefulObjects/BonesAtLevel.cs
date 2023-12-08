using Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Logic.UsefulObjects
{
    public class BonesAtLevel : MonoBehaviour
    {
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IPersistentProgressService progressService) =>
            _progressService = progressService;

        public void AddBones() =>
            _progressService.GetUserProgress.Bones += 1;
    }
}