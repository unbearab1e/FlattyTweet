
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;

namespace FlattyTweet.Extensions
{
  public class PropertyObserver<TPropertySource> : IWeakEventListener where TPropertySource : INotifyPropertyChanged
  {
    private readonly Dictionary<string, Action<TPropertySource>> _propertyNameToHandlerMap;
    private readonly WeakReference _propertySourceRef;

    public PropertyObserver(TPropertySource propertySource)
    {
      if ((object) propertySource == null)
        throw new ArgumentNullException("propertySource");
      this._propertySourceRef = new WeakReference((object) propertySource);
      this._propertyNameToHandlerMap = new Dictionary<string, Action<TPropertySource>>();
    }

    public PropertyObserver<TPropertySource> RegisterHandler(System.Linq.Expressions.Expression<Func<TPropertySource, object>> expression, Action<TPropertySource> handler)
    {
      if (expression == null)
        throw new ArgumentNullException("expression");
      string propertyName = PropertyObserver<TPropertySource>.GetPropertyName(expression);
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentException("'expression' did not provide a property name.");
      if (handler == null)
        throw new ArgumentNullException("handler");
      TPropertySource propertySource = this.GetPropertySource();
      if ((object) propertySource != null)
      {
        Debug.Assert(!this._propertyNameToHandlerMap.ContainsKey(propertyName), "Why is the '" + propertyName + "' property being registered again?");
        this._propertyNameToHandlerMap[propertyName] = handler;
        PropertyChangedEventManager.AddListener((INotifyPropertyChanged) propertySource, (IWeakEventListener) this, propertyName);
      }
      return this;
    }

    public PropertyObserver<TPropertySource> UnregisterHandler(System.Linq.Expressions.Expression<Func<TPropertySource, object>> expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression");
      string propertyName = PropertyObserver<TPropertySource>.GetPropertyName(expression);
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentException("'expression' did not provide a property name.");
      TPropertySource propertySource = this.GetPropertySource();
      if ((object) propertySource != null && this._propertyNameToHandlerMap.ContainsKey(propertyName))
      {
        this._propertyNameToHandlerMap.Remove(propertyName);
        PropertyChangedEventManager.RemoveListener((INotifyPropertyChanged) propertySource, (IWeakEventListener) this, propertyName);
      }
      return this;
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedEventManager))
      {
        PropertyChangedEventArgs changedEventArgs = e as PropertyChangedEventArgs;
        if (changedEventArgs != null && sender is TPropertySource)
        {
          string propertyName = changedEventArgs.PropertyName;
          TPropertySource propertySource = (TPropertySource) sender;
          if (string.IsNullOrEmpty(propertyName))
          {
            foreach (Action<TPropertySource> action in Enumerable.ToArray<Action<TPropertySource>>((IEnumerable<Action<TPropertySource>>) this._propertyNameToHandlerMap.Values))
              action(propertySource);
            flag = true;
          }
          else
          {
            Action<TPropertySource> action;
            if (this._propertyNameToHandlerMap.TryGetValue(propertyName, out action))
            {
              action(propertySource);
              flag = true;
            }
          }
        }
      }
      return flag;
    }

    private static string GetPropertyName(System.Linq.Expressions.Expression<Func<TPropertySource, object>> expression)
    {
      LambdaExpression lambdaExpression = (LambdaExpression) expression;
      MemberExpression memberExpression = !(lambdaExpression.Body is UnaryExpression) ? lambdaExpression.Body as MemberExpression : (lambdaExpression.Body as UnaryExpression).Operand as MemberExpression;
      Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");
      if (memberExpression != null)
        return (memberExpression.Member as PropertyInfo).Name;
      else
        return (string) null;
    }

    private TPropertySource GetPropertySource()
    {
      try
      {
        return (TPropertySource) this._propertySourceRef.Target;
      }
      catch
      {
        return default (TPropertySource);
      }
    }
  }
}
