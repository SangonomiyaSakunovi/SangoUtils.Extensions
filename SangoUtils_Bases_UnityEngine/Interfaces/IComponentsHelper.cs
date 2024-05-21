using System;

namespace SangoUtils.Bases_Unity.ComponentsHelpers
{
    public interface IComponentsHelper
    {
        void OnInitialize();

        Type[] GetReleventComponents();
    }
}
