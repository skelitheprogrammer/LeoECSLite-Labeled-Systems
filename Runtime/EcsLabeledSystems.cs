using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;

namespace Skillitronic.LeoECSLite.LabeledSystems
{
    public sealed class EcsLabeledSystems : IEcsLabeledSystems
    {
        private readonly EcsWorld _defaultWorld;
        private readonly Dictionary<string, EcsWorld> _worlds;

        private readonly List<string> _labels;
        
        private readonly Dictionary<string, List<IEcsSystem>> _allSystems;

        private readonly Dictionary<string, List<IEcsRunSystem>> _sortedRunSystems;
        private readonly Dictionary<string, List<IEcsPostRunSystem>> _sortedPostRunSystems;

        readonly object _shared;

        public EcsLabeledSystems(EcsWorld defaultWorld, object shared = null)
        {
            _defaultWorld = defaultWorld;

            _shared = shared;
            
            _labels = new();
            _worlds = new();
            _allSystems = new();
            _sortedRunSystems = new();
            _sortedPostRunSystems = new();
        }

        public IEcsLabeledSystems AddLabel(string markerName)
        {
            if (_labels.Contains(markerName))
            {
                throw new ArgumentException();
            }
            
            _labels.Add(markerName);
            _allSystems.Add(markerName, new List<IEcsSystem>());
            _sortedRunSystems.Add(markerName, new List<IEcsRunSystem>());
            _sortedPostRunSystems.Add(markerName, new List<IEcsPostRunSystem>());

            return this;
        }
        
        public IEcsLabeledSystems AddAt(IEcsSystem system, string label)
        {
            _allSystems[label].Add(system);

            if (system is IEcsRunSystem runSystem)
            {
                _sortedRunSystems[label].Add(runSystem);
            }

            if (system is IEcsPostRunSystem postRunSystem)
            {
                _sortedPostRunSystems[label].Add(postRunSystem);
            }

            return this;
        }

        public IEcsSystems AddWorld(EcsWorld world, string name)
        {
            _worlds[name] = world;
            return this;
        }

        public EcsWorld GetWorld(string name = null)
        {
            if (name == null)
            {
                return _defaultWorld;
            }

            _worlds.TryGetValue(name, out EcsWorld world);
            return world;
        }

        public Dictionary<string, EcsWorld> GetAllNamedWorlds()
        {
            return _worlds;
        }

        public IEcsSystems Add(IEcsSystem system)
        {
            AddAt(system, _labels[^1]);
            return this;
        }

        public void Init()
        {
            for (int index = 0; index < _labels.Count; index++)
            {
                string label = _labels[index];
                int j;

                for (j = 0; j < _allSystems[label].Count; j++)
                {
                    IEcsSystem system = _allSystems[label][j];
                    if (system is IEcsPreInitSystem initSystem)
                    {
                        initSystem.PreInit(this);
                    }
                }

                for (j = 0; j < _allSystems[label].Count; j++)
                {
                    IEcsSystem system = _allSystems[label][j];
                    if (system is IEcsInitSystem initSystem)
                    {
                        initSystem.Init(this);
                    }
                }
            }
        }

        public void Run()
        {
            for (int index = 0; index < _labels.Count; index++)
            {
                string label = _labels[index];

                for (int j = 0; j < _sortedRunSystems[label].Count; j++)
                {
                    _sortedRunSystems[label][j].Run(this);
                }

                for (int j = 0; j < _sortedPostRunSystems[label].Count; j++)
                {
                    _sortedPostRunSystems[label][j].PostRun(this);
                }
            }
        }

        public List<IEcsSystem> GetAllSystems()
        {
            return _allSystems.Values.SelectMany(c => c).ToList();
        }

        public TShared GetShared<TShared>() where TShared : class
        {
            return _shared as TShared;
        }

        public void Destroy()
        {
            for (int index = 0; index < _labels.Count; index++)
            {
                string label = _labels[index];
                int j;

                for (j = 0; j < _allSystems[label].Count; j++)
                {
                    IEcsSystem system = _allSystems[label][j];
                    if (system is IEcsDestroySystem initSystem)
                    {
                        initSystem.Destroy(this);
                    }
                }

                for (j = 0; j < _allSystems[label].Count; j++)
                {
                    IEcsSystem system = _allSystems[label][j];
                    if (system is IEcsPostDestroySystem initSystem)
                    {
                        initSystem.PostDestroy(this);
                    }
                }
            }
            
            _labels.Clear();
            _worlds.Clear();
            _sortedRunSystems.Clear();
            _sortedPostRunSystems.Clear();
        }
    }
}