using System.Collections.Generic;
using k.LevelService.Interfaces;

namespace k.LevelService
{
    public class LevelController
    {
        private readonly List<ILevel> _registeredLevels = new();
        private ILevel _activeLevel;
        
        public void RegisterLevel(ILevel level)
        {
            if (_registeredLevels.Contains(level)) return;
            _registeredLevels.Add(level);
        }
        
        public void UnregisterLevel(ILevel level)
        {
            if (!_registeredLevels.Contains(level)) return;
            if (_activeLevel == level)
            {
                UnloadActiveLevel();
            }
            _registeredLevels.Remove(level);
        }
        
        public void ClearLevels()
        {
            UnloadActiveLevel();
            _registeredLevels.Clear();
        }
        
        public bool LoadLevel(ILevel level, bool unloadActiveLevel = true)
        {
            if (!_registeredLevels.Contains(level)) return false;
            if (unloadActiveLevel) UnloadActiveLevel();
            
            level.Load();
            _activeLevel = level;
            return true;
        }
        
        public bool UnloadLevel(ILevel level)
        {
            if (!_registeredLevels.Contains(level)) return false;
            level.Unload();
            if (_activeLevel == level) _activeLevel = null;
            return true;
        }
        
        public bool UnloadActiveLevel()
        {
            if (_activeLevel == null) return false;
            
            _activeLevel.Unload();
            _activeLevel = null;
            return true;
        }
    }
}