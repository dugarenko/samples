using Microsoft.Xaml.Behaviors;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace MVVMCore.Behaviors
{
    /// <summary>
    /// Behavior that will connect an UI event to a viewmodel Command,
    /// allowing the event arguments to be passed as the CommandParameter.
    /// </summary>
    public class EventBehavior : Behavior<FrameworkElement>
    {
        #region Declarations.

        private static Type _thisType = typeof(EventBehavior);
        private static MethodInfo _executeCommandMethodInfo = _thisType.GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
        private EventInfo _eventInfo = null;
        private Delegate _eventHandler = null;

        #endregion

        #region DependencyProperty.

        /// <summary>
        /// DependencyProperty for EventName property.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName",
            typeof(string), _thisType, new PropertyMetadata(null, OnEventChanged));

        /// <summary>
        /// DependencyProperty for Command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), _thisType, new PropertyMetadata(null));

        /// <summary>
        /// DependencyProperty for PassEventArgs property.
        /// </summary>
        public static readonly DependencyProperty PassEventArgsProperty = DependencyProperty.Register("PassEventArgs",
            typeof(bool), _thisType, new PropertyMetadata(false));

        /// <summary>
        /// DependencyProperty for CommandParameter property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter",
            typeof(object), _thisType, new PropertyMetadata(null));

        #endregion

        #region Private methods - PropertyChangedCallback.

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EventBehavior eb = d as EventBehavior;
            if (eb != null)
            {
                eb.AttachHandler((string)e.NewValue);
            }
        }

        #endregion

        #region Private methods.

        /// <summary>
        /// Attach the handler to the event.
        /// </summary>
        /// <param name="eventName">A event name.</param>
        private void AttachHandler(string eventName)
        {
            if (this.AssociatedObject != null && string.IsNullOrEmpty(eventName) == false)
            {
                this.DetachHandler();
                _eventInfo = this.AssociatedObject.GetType().GetEvent(eventName);
                if (_eventInfo != null)
                {
                    _eventHandler = Delegate.CreateDelegate(_eventInfo.EventHandlerType, this, _executeCommandMethodInfo);
                    _eventInfo.AddEventHandler(this.AssociatedObject, _eventHandler);
                }
                else
                {
                    throw new ArgumentException(string.Format("The event '{0}' was not found on type '{1}'.", eventName, this.AssociatedObject.GetType().Name));
                }
            }
        }

        /// <summary>
        /// Detach the event handler.
        /// </summary>
        private void DetachHandler()
        {
            if (this.AssociatedObject != null && _eventInfo != null && _eventHandler != null)
            {
                _eventInfo.RemoveEventHandler(this.AssociatedObject, _eventHandler);
                _eventInfo = null;
                _eventHandler = null;
            }
        }

        #endregion

        #region Protected methods.

        /// <summary>
        /// Execute the Command.
        /// </summary>
        protected virtual void ExecuteCommand(object sender, EventArgs e)
        {
            object parameter = null;

            if (this.PassEventArgs)
            {
                parameter = e;
            }
            else
            {
                parameter = CommandParameter;
            }

            if (this.Command != null && this.Command.CanExecute(parameter))
            {
                this.Command.Execute(parameter);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AttachHandler(this.EventName);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.DetachHandler();
        }

        #endregion

        #region Public properties.

        public virtual string EventName
        {
            get
            {
                return (string)GetValue(EventNameProperty);
            }
            set
            {
                SetValue(EventNameProperty, value);
            }
        }

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public bool PassEventArgs
        {
            get
            {
                return (bool)GetValue(PassEventArgsProperty);
            }
            set
            {
                SetValue(PassEventArgsProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        #endregion
    }
}
