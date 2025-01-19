using k.LevelService.Common.Interfaces;
using UnityEngine;

namespace k.LevelService.Common.Implementations
{
    public class BaseScriptableLevel : ScriptableObject, ILevel
    {
        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }
    }
}