using System;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    public interface IPropertyCollection
    {
        T GetProperty<T>(string key);
    }

    public class PropertyCollection : IPropertyCollection
    {
        // okay not sure if these make absolutely ANY SENSE but whatever.. no boxing at least, yay?
        private abstract class Property { }
        private sealed class Property<T> : Property
        {
            public T Value;
        }

        // todo: this causes boxing with value types, could do some kind of Property<T> wrapper tms
        private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();

        public bool HasProperty(string key)
        {
            return _properties.ContainsKey(key);
        }

        public T GetProperty<T>(string key)
        {
            return ((Property<T>)_properties[key]).Value;
        }

        public void SetProperty<T>(string key, T value)
        {
            if (this.HasProperty(key))
            {
                ((Property<T>)_properties[key]).Value = value;
            }
            else
            {
                _properties.Add(key, new Property<T> { Value = value });
            }
        }
    }

    public class ReadOnlyPropertyCollection : IPropertyCollection
    {
        private readonly IPropertyCollection _propertyCollection;

        public ReadOnlyPropertyCollection(IPropertyCollection propertyCollection)
        {
            _propertyCollection = propertyCollection;
        }

        public T GetProperty<T>(string key)
        {
            return _propertyCollection.GetProperty<T>(key);
        }
    }
}