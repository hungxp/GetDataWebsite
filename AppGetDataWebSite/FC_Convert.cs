using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

using System.Reflection;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppGetDataWebSite
{
    class FC_Convert
    {
        #region Parse - Convert

        public static int ParseInt(object obj)
        {
            try
            {
                int result;
                if (obj == null) return 0;
                if (int.TryParse(obj.ToString(), out result))
                    return result;
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ParseSesson(object obj)
        {
            try
            {
                int resutl = 0;
                switch (obj.ToString()) { 
                    case "06":
                        resutl = 6;
                        break;
                    case "07":
                        resutl = 7;
                        break;
                    case "08":
                        resutl = 8;
                        break;
                    case "09":
                        resutl = 9;
                        break;
                    case "10":
                        resutl = 10;
                        break;
                    case "11":
                        resutl = 11;
                        break;
                    case "12":
                        resutl = 12;
                        break;
                    case "13":
                        resutl = 13;
                        break;
                    case "14":
                        resutl = 14;
                        break;
                    case "15":
                        resutl = 15;
                        break;
                    case "16":
                        resutl = 16;
                        break;                  
                    case "wc":
                        resutl = 2;
                        break;
                    case "xi":
                        resutl = 3;
                        break;
                    default:
                        resutl = 1;
                        break;
                }
                return resutl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime ParseBirthDay(object obj)
        {
            try
            {
                DateTime result;
                if (obj == null) return DateTime.Now;
                else
                {
                    string[] strArr = obj.ToString().Split('/');
                    string str = strArr[1] + "/" + strArr[0] + "/" + strArr[2];
                    result = DateTime.Parse(str);
                   // result = DateTime.ParseExact(obj.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);                   
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long ParseLong(object obj)
        {
            try
            {
                long result;
                if (obj == null) return 0;
                if (long.TryParse(obj.ToString(), out result))
                    return result;
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double ParseDouble(object obj)
        {
            try
            {
                double result;
                if (obj == null) return 0;
                if (double.TryParse(obj.ToString(), out result))
                    return result;
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal ParseDecimal(object obj)
        {
            try
            {
                decimal result;
                if (obj == null) return 0;
                if (decimal.TryParse(obj.ToString(), out result))
                    return result;
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static float ParseFloat(object obj)
        {
            try
            {
                float result;
                if (obj == null) return 0;
                if (float.TryParse(obj.ToString(), out result))
                    return result;
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ToString(object obj)
        {
            try
            {
                if (obj == null) return string.Empty;
                return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ParseBool(object obj)
        {
            try
            {
                bool result;
                if (obj == null) return false;
                if (bool.TryParse(obj.ToString(), out result))
                    return result;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static DateTime? ParseDateTime(object obj)
        {
            try
            {
                DateTime result;
                if (obj == null) return null;
                if (obj != null && DateTime.Parse(obj.ToString()).Year == 1899) return null;
                if (obj != null && DateTime.Parse(obj.ToString()).Year == 1900) return null;
                if (DateTime.TryParse(obj.ToString(), out result))
                    return result;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime ParseDateTimes(object obj)
        {
            try
            {
                DateTime result;
                if (obj == null) return DateTime.Now;
                if (obj != null && DateTime.Parse(obj.ToString()).Year == 1899) return DateTime.Now;
                if (obj != null && DateTime.Parse(obj.ToString()).Year == 1900) return DateTime.Now;
                if (DateTime.TryParse(obj.ToString(), out result))
                    return result;
                return DateTime.Now;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static char? ParseChar(object obj)
        {
            try
            {
                char result;
                if (obj == null) return Convert.ToChar(" ");
                if (char.TryParse(obj.ToString(), out result))
                    return result;
                return null;
            }
            catch (Exception)
            {
                return Convert.ToChar(" ");
            }
        }

        public static string CaseSensitive(string source)
        {
            try
            {
                return Regex.Replace(source, "[A-Z]", " $&");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataRow ClassToDataRow<T>(T Data) where T : class
        {
            DataTable Table = new DataTable();
            Type classType = typeof(T);
            DataRow row = Table.NewRow();
            string className = classType.UnderlyingSystemType.Name;
            List<PropertyInfo> propertyList = classType.GetProperties().ToList();
            foreach (PropertyInfo prop in propertyList)
                Table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (PropertyInfo prop in propertyList)
            {
                if (Table.Columns.Contains(prop.Name))
                {
                    if (Table.Columns[prop.Name] != null)
                    {
                        row[prop.Name] = prop.GetValue(Data, null);
                    }
                }
            }

            return row;
        }

        public static List<SqlParameter> ClassToSqlParameter<T>(T Data) where T : class
        {
            List<SqlParameter> lst = new List<SqlParameter>();
            Type classType = typeof(T);
        
            string className = classType.UnderlyingSystemType.Name;
            List<PropertyInfo> propertyList = classType.GetProperties().ToList();
       
            foreach (PropertyInfo prop in propertyList)
            {
                SqlParameter pr = new SqlParameter();
                pr.ParameterName = "@" + prop.Name;
                pr.Value = prop.GetValue(Data, null);                
                lst.Add(pr);
            }

            return lst;
        }

        public static List<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }

        }
        private static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }
        
        #endregion
    }  
}
