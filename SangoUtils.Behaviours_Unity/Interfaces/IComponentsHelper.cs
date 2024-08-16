using System;

namespace SangoUtils.Behaviours_Unity.ComponentsHelpers
{
    public interface IComponentsHelper
    {
        void OnInitialize();

        Type[] GetReleventComponents();
    }
}
