using System;
using System.Collections.ObjectModel;

namespace MVVMCore.Commands
{
    [AttributeUsage(AttributeTargets.Parameter)]
	public sealed class ValidatedNotNullAttribute : Attribute
    {
        public ValidatedNotNullAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public sealed class ValueDependsOnCollectionAttribute : Attribute
    {
        public ValueDependsOnCollectionAttribute(string sourceName)
        {
            SourceName = sourceName;
        }

        public string SourceName { get; }

        public override object TypeId
        {
            get
            {
                return this;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public sealed class ValueDependsOnExternalPropertyAttribute : Attribute
    {
        public ValueDependsOnExternalPropertyAttribute(string sourceName)
            : this(sourceName, string.Empty)
        { }

        public ValueDependsOnExternalPropertyAttribute(string sourceName, string propertyName)
        {
            SourceName = sourceName;
            PropertyName = propertyName ?? string.Empty;
        }

        public string PropertyName { get; } = string.Empty;

        public string SourceName { get; }

        public override object TypeId
        {
            get
            {
                return this;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public abstract class DependsOnPropertyAttribute : Attribute
    {
        protected DependsOnPropertyAttribute(string propertyName)
        {
            PropertyNames = new ReadOnlyCollection<string>(new string[] { propertyName });
        }

        protected DependsOnPropertyAttribute(string propertyName1, string propertyName2)
        {
            ArgumentValidation.NotNullOrEmpty<char>(propertyName1, "propertyName1");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName2, "propertyName2");
            PropertyNames = new ReadOnlyCollection<string>(new string[] { propertyName1, propertyName2 });
        }

        protected DependsOnPropertyAttribute(string propertyName1, string propertyName2, string propertyName3)
        {
            ArgumentValidation.NotNullOrEmpty<char>(propertyName1, "propertyName1");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName2, "propertyName2");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName3, "propertyName3");
            PropertyNames = new ReadOnlyCollection<string>(new string[] { propertyName1, propertyName2, propertyName3 });
        }

        protected DependsOnPropertyAttribute(string propertyName1, string propertyName2, string propertyName3, string propertyName4)
        {
            ArgumentValidation.NotNullOrEmpty<char>(propertyName1, "propertyName1");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName2, "propertyName2");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName3, "propertyName3");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName4, "propertyName4");
            PropertyNames = new ReadOnlyCollection<string>(new string[] { propertyName1, propertyName2, propertyName3, propertyName4 });
        }

        protected DependsOnPropertyAttribute(string propertyName1, string propertyName2, string propertyName3, string propertyName4, string propertyName5)
        {
            ArgumentValidation.NotNullOrEmpty<char>(propertyName1, "propertyName1");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName2, "propertyName2");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName3, "propertyName3");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName4, "propertyName4");
            ArgumentValidation.NotNullOrEmpty<char>(propertyName5, "propertyName5");
            PropertyNames = new ReadOnlyCollection<string>(new string[] { propertyName1, propertyName2, propertyName3, propertyName4, propertyName5 });
        }

        public ReadOnlyCollection<string> PropertyNames { get; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class ValueDependsOnPropertyAttribute : DependsOnPropertyAttribute
    {
        public ValueDependsOnPropertyAttribute(string propertyName)
            : base(propertyName)
        { }

        public ValueDependsOnPropertyAttribute(string propertyName1, string propertyName2)
            : base(propertyName1, propertyName2)
        { }

        public ValueDependsOnPropertyAttribute(string propertyName1, string propertyName2, string propertyName3)
            : base(propertyName1, propertyName2, propertyName3)
        { }

        public ValueDependsOnPropertyAttribute(string propertyName1, string propertyName2, string propertyName3, string propertyName4)
            : base(propertyName1, propertyName2, propertyName3, propertyName4)
        { }

        public ValueDependsOnPropertyAttribute(string propertyName1, string propertyName2, string propertyName3, string propertyName4, string propertyName5)
            : base(propertyName1, propertyName2, propertyName3, propertyName4, propertyName5)
        { }
    }
}
