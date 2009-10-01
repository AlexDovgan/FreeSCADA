using System;
using System.Collections.Generic;
using System.Windows.Data;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace FreeSCADA.Common.Schema
{
    public class ScriptConverter : IMultiValueConverter
    {
        public string Expression
        {
            get;
            set;
        }
        static ScriptEngine python = InitializePython();
        public ScriptConverter()
        {
        }

        public ScriptConverter(string exp)
        {
            Expression = exp;
        }
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string pyExpr = "from math import *\n" + Expression;
            ScriptSource source= python.CreateScriptSourceFromString(pyExpr, Microsoft.Scripting.SourceCodeKind.Statements);;
            ScriptScope scope = python.CreateScope();
            scope.SetVariable("values",values);
            scope.SetVariable("result", Binding.DoNothing  );
            //scope.SetVariable("object", obj);

            if (source != null)
            {
                try
                {
                    source.Execute(scope);
                }
                catch (System.Exception e)
                {
                    Env.Current.Logger.LogWarning(string.Format("ScriptConverter: Failed to execute script converter on : {0}, Error: {1}", parameter, e.Message));
                    return null;
                }
            }
            return scope.GetVariable("result");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value;
        }
        static ScriptEngine InitializePython()
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options["DivisionOptions"] = IronPython.PythonDivisionOptions.New;
            return Python.CreateEngine(options);
        }

       

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
