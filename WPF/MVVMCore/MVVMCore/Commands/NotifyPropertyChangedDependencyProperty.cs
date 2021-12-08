using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MVVMCore.Commands
{
    [Serializable]
	public abstract class NotifyPropertyChangedDependencyProperty : IWeakEventListener
    {
        #region Declarations.

        private readonly Lazy<Dictionary<INotifyPropertyChanged, string>> _externalPropertySources = new Lazy<Dictionary<INotifyPropertyChanged, string>>();
        private readonly Lazy<Dictionary<INotifyCollectionChanged, string>> _collectionSources = new Lazy<Dictionary<INotifyCollectionChanged, string>>();
        private readonly Dictionary<string, List<string>> _internalPropertyDependencies;
        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> _externalPropertyDependencies;
        private readonly Dictionary<string, HashSet<string>> _collectionPropertyDependencies;

        #endregion

        protected NotifyPropertyChangedDependencyProperty()
        {
            _internalPropertyDependencies = BuildInternalPropertyDependencies();
            _externalPropertyDependencies = BuildExternalPropertyDependencies();
            _collectionPropertyDependencies = BuildCollectionPropertyDependencies();
        }

        private Dictionary<string, HashSet<string>> BuildCollectionPropertyDependencies()
        {
            HashSet<string> values;
            Dictionary<string, HashSet<string>> sources = null;
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo propertyInfo = properties[i];
                foreach (ValueDependsOnCollectionAttribute customAttribute in propertyInfo.GetCustomAttributes<ValueDependsOnCollectionAttribute>(false))
                {
                    if (sources == null)
                    {
                        sources = new Dictionary<string, HashSet<string>>();
                    }
                    if (!sources.TryGetValue(customAttribute.SourceName, out values))
                    {
                        values = new HashSet<string>();
                        sources.Add(customAttribute.SourceName, values);
                    }
                    values.Add(propertyInfo.Name);
                }
            }
            return sources;
        }

        private Dictionary<string, Dictionary<string, HashSet<string>>> BuildExternalPropertyDependencies()
        {
            Dictionary<string, HashSet<string>> values;
            HashSet<string> values2;
            Dictionary<string, Dictionary<string, HashSet<string>>> sources = null;
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo propertyInfo = properties[i];
                foreach (ValueDependsOnExternalPropertyAttribute customAttribute in propertyInfo.GetCustomAttributes<ValueDependsOnExternalPropertyAttribute>(false))
                {
                    if (sources == null)
                    {
                        sources = new Dictionary<string, Dictionary<string, HashSet<string>>>();
                    }
                    if (!sources.TryGetValue(customAttribute.SourceName, out values))
                    {
                        values = new Dictionary<string, HashSet<string>>();
                        sources.Add(customAttribute.SourceName, values);
                    }
                    if (!values.TryGetValue(customAttribute.PropertyName, out values2))
                    {
                        values2 = new HashSet<string>();
                        values.Add(customAttribute.PropertyName, values2);
                    }
                    values2.Add(propertyInfo.Name);
                }
            }
            return sources;
        }

        private Dictionary<string, List<string>> BuildInternalPropertyDependencies()
        {
            List<string> values;
            Dictionary<string, List<string>> sources = null;
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo propertyInfo = properties[i];
                DependsOnPropertyAttribute[] array = propertyInfo.GetCustomAttributes(typeof(DependsOnPropertyAttribute), true).Cast<DependsOnPropertyAttribute>().ToArray<DependsOnPropertyAttribute>();
                for (int j = 0; j < (int)array.Length; j++)
                {
                    DependsOnPropertyAttribute dependsOnPropertyAttribute = array[j];
                    if (sources == null)
                    {
                        sources = new Dictionary<string, List<string>>();
                    }
                    foreach (string propertyName in dependsOnPropertyAttribute.PropertyNames)
                    {
                        string empty = propertyName;
                        if (string.IsNullOrEmpty(empty))
                        {
                            empty = string.Empty;
                        }
                        if (!sources.TryGetValue(empty, out values))
                        {
                            values = new List<string>();
                            sources.Add(empty, values);
                        }
                        values.Add(propertyInfo.Name);
                    }
                }
            }
            return sources;
        }

        protected abstract void RaisePropertyChanged<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, params string[] propertiesToExclude) where TValue : IEnumerable<string>;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (_internalPropertyDependencies == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                RaisePropertyChanged(_internalPropertyDependencies, propertyName, new string[0]);
                RaisePropertyChanged(_internalPropertyDependencies, string.Empty, new string[] { propertyName });
            }
        }

        protected void AddDependencySource(string name, INotifyPropertyChanged source)
        {
            ArgumentValidation.NotNullOrEmpty<char>(name, "name");
            ArgumentValidation.NotNull(source, "source");
            _externalPropertySources.Value.Add(source, name);
            PropertyChangedEventManager.AddListener(source, this, string.Empty);
        }

        protected void AddDependencySource(string name, INotifyCollectionChanged source)
        {
            ArgumentValidation.NotNullOrEmpty<char>(name, "name");
            ArgumentValidation.NotNull(source, "source");
            _collectionSources.Value.Add(source, name);
            CollectionChangedEventManager.AddListener(source, this);
        }

        protected void RemoveDependencySource(string name, INotifyPropertyChanged source)
        {
            ArgumentValidation.NotNullOrEmpty<char>(name, "name");
            ArgumentValidation.NotNull(source, "source");
            PropertyChangedEventManager.RemoveListener(source, this, string.Empty);
            _externalPropertySources.Value.Remove(source);
        }

        protected void RemoveDependencySource(string name, INotifyCollectionChanged source)
        {
            ArgumentValidation.NotNullOrEmpty<char>(name, "name");
            ArgumentValidation.NotNull(source, "source");
            CollectionChangedEventManager.RemoveListener(source, this);
            _collectionSources.Value.Remove(source);
        }

        protected string GetDependencySourceName(INotifyPropertyChanged source)
        {
            string value = null;
            if (_externalPropertySources.IsValueCreated)
            {
                _externalPropertySources.Value.TryGetValue(source, out value);
            }
            return value;
        }

        protected string GetDependencySourceName(INotifyCollectionChanged source)
        {
            string value = null;
            if (_collectionSources.IsValueCreated)
            {
                _collectionSources.Value.TryGetValue(source, out value);
            }
            return value;
        }

        protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(PropertyChangedEventManager))
            {
                if (managerType != typeof(CollectionChangedEventManager))
                {
                    return false;
                }
                string dependencySourceName = GetDependencySourceName((INotifyCollectionChanged)sender);
                if (dependencySourceName != null)
                {
                    RaisePropertyChanged(_collectionPropertyDependencies, dependencySourceName, new string[0]);
                }
                return true;
            }

            Dictionary<string, HashSet<string>> propertyDependencies;
            INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)sender;
            string sourceName = GetDependencySourceName(notifyPropertyChanged);

            if (sourceName != null && _externalPropertyDependencies != null && _externalPropertyDependencies.TryGetValue(sourceName, out propertyDependencies))
            {
                PropertyChangedEventArgs propertyChangedEventArg = (PropertyChangedEventArgs)e;
                if (string.IsNullOrEmpty(propertyChangedEventArg.PropertyName))
                {
                    foreach (HashSet<string> value in propertyDependencies.Values)
                    {
                        foreach (string propertyName in value)
                        {
                            RaisePropertyChanged(propertyName);
                        }
                    }
                }
                else
                {
                    RaisePropertyChanged(propertyDependencies, propertyChangedEventArg.PropertyName, new string[0]);
                    RaisePropertyChanged(propertyDependencies, string.Empty, new string[0]);
                }
            }
            return true;
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (ReceiveWeakEvent(managerType, sender, e))
            {
                return true;
            }
            return false;
        }
    }
}
