﻿using Leopotam.EcsLite;

namespace Skillitronic.LeoECSLite.LabeledSystems
{
    public interface IEcsLabeledSystems : IEcsSystems
    {
        IEcsLabeledSystems AddAt(IEcsSystem system, string marker);
        IEcsLabeledSystems AddMarker(string marker);
    }
}