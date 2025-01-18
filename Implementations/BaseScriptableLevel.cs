using k.LevelService.Interfaces;
using UnityEngine;

namespace k.LevelService.Implementations
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