using System;

namespace SangoUtils.Bases_Unity.ComponentHelpers
{
    public interface IComponentsHelper
    {
        void OnInitialize();

        Type[] GetReleventComponents();
    }
}
