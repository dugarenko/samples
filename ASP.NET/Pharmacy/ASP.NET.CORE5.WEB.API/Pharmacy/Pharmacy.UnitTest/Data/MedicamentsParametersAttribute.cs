using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Pharmacy.UnitTest.Data
{
    public class MedicamentsParametersAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            switch (methodInfo.Name)
            {
                case "GetAll_PositiveTest":
                    {
                        foreach (var p in AppDbContextPositive.Medicaments_Positive_GetAll_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;
                case "Get_PositiveTest":
                    {
                        foreach (var p in AppDbContextPositive.Medicaments_Positive_Get_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;
                case "Put_PositiveTest":
                    {
                        foreach (var p in AppDbContextPositive.Medicaments_Positive_Put_Parameters())
                        {
                            yield return new object[] { p.Key, p.Value };
                        }
                    }
                    break;
                case "Post_PositiveTest":
                    {
                        foreach (var p in AppDbContextPositive.Medicaments_Positive_Post_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;
                case "Delete_PositiveTest":
                    {
                        foreach (var p in AppDbContextPositive.Medicaments_Positive_Delete_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;


                case "GetAll_NegativeTest":
                    {
                        foreach (var p in AppDbContextNegative.Medicaments_Negative_GetAll_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;
                case "Get_NegativeTest":
                    {
                        foreach (var p in AppDbContextNegative.Medicaments_Negative_Get_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;
                case "Put_NegativeTest":
                    {
                        foreach (var p in AppDbContextNegative.Medicaments_Negative_Put_Parameters())
                        {
                            yield return new object[] { p.Key, p.Value };
                        }
                    }
                    break;
                case "Post_NegativeTest":
                    {
                        foreach (var p in AppDbContextNegative.Medicaments_Negative_Post_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;
                case "Delete_NegativeTest":
                    {
                        foreach (var p in AppDbContextNegative.Medicaments_Negative_Delete_Parameters())
                        {
                            yield return new object[] { p };
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException($"Not implemented cases: '{methodInfo.Name}' in method '{nameof(GetData)}'.");
            }
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null && data.Length > 0)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data));
            }
            return methodInfo.Name;
        }
    }
}
