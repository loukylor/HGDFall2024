using System;
using System.Reflection;
using UnityEngine;

namespace HGDFall2024.Managers
{
    public abstract class BaseManager : MonoBehaviour
    {
        private MethodInfo _setter;

        protected virtual void Awake()
        {
            SetSingleton();

            // Mark this as do not destroy on load
            DontDestroyOnLoad(this);    
        }

        protected virtual void OnDestroy()
        {
            Debug.LogWarning(GetType().Name + " has been destroyed");
            _setter.Invoke(this, new object[1] { null });
            _setter = null;
        }

        private void SetSingleton()
        {
            // Use reflection to populate instance setter
            PropertyInfo prop = GetType().GetProperty("Instance", BindingFlags.Static | BindingFlags.Public) 
                ?? throw new InvalidOperationException("Children of BaseManager must have an instance property");

            if (_setter != null)
            {
                throw new InvalidOperationException(GetType().Name + " has been initialized twice");
            }
            _setter = prop.GetSetMethod(true);
            if (_setter == null)
            {
                throw new InvalidOperationException("Children of BaseManager must have a setter on the instance property");
            }
            _setter.Invoke(this, new object[1] { this });
        }
    }
}
