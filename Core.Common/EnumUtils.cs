using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Reflection;

/*
 * 来源网络
 * 具体参考：http://bbs.csdn.net/topics/390204077
 */
namespace Core.Common
{
    /// <summary>
    /// 提供对枚举操作的一些方法。
    /// </summary>
    public class EnumUtils
    {
        /// <summary>
        /// 用于缓存枚举值的属性值
        /// </summary>
        private static readonly Dictionary<object, EnumAttribute> enumAttr = new Dictionary<object, EnumAttribute>();

        /// <summary>
        /// 获取枚举值的名称，该名称由EnumAttribute定义
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举值对应的名称</returns>
        public static string GetName(Enum value)
        {
            EnumAttribute ea = GetAttribute(value);
            return ea != null ? ea.Name : "";
        }

        /// <summary>
        /// 获取枚举值的名称，该名称由EnumAttribute定义
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举值对应的名称</returns>
        public static string GetDescription(Enum value)
        {
            EnumAttribute ea = GetAttribute(value);
            return ea != null ? ea.Description : "";
        }

        /// <summary>
        /// 利用检举填充控件列表<br/>
        /// 该方法取枚举的名称和值生成ListItem添后添加到列表中
        /// </summary>
        /// <param name="list">控件列表</param>
        /// <param name="enumType">枚举类型</param>
        public static void FillList(ListItemCollection list, Type enumType)
        {
            FillList(list, enumType, null, null);
        }

        /// <summary>
        /// 利用检举填充控件列表<br/>
        /// 该方法取枚举的名称和值生成ListItem添后添加到列表中
        /// </summary>
        /// <param name="list">控件列表</param>
        /// <param name="enumType">枚举类型</param>
        /// <param name="headerValue">列表头的值</param>
        /// <param name="headerText">列表头的显示文字</param>
        public static void FillList(ListItemCollection list, Type enumType, string headerValue, string headerText)
        {
            if (headerValue != null || headerText != null) list.Add(new ListItem(headerText, headerValue));
            Dictionary<string, string> dic = GetValueName(enumType);
            foreach (KeyValuePair<string, string> kv in dic)
            {
                list.Add(new ListItem(kv.Value, kv.Key));
            }
        }

        /// <summary>
        /// 获取枚举类型的 值-名称 列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetValueName(Type enumType)
        {
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (object o in Enum.GetValues(enumType))
            {
                Enum e = (Enum)o;
                string value = Convert.ChangeType(o, underlyingType).ToString();
                dic.Add(value, GetName(e));
            }
            return dic;
        }

        /// <summary>
        /// 从字符串转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型（包括可空枚举）</typeparam>
        /// <param name="str">要转为枚举的字符串</param>
        /// <exception cref="Exception">转换失败</exception>
        /// <returns>转换结果</returns>
        public static T GetEnum<T>(string str)
        {
            Type type = typeof(T);

            Type nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null) type = nullableType;

            Type underlyingType = Enum.GetUnderlyingType(type);
            object o = Convert.ChangeType(str, underlyingType);

            if (!Enum.IsDefined(type, o)) throw new Exception("枚举类型\"" + type.ToString() + "\"中没有定义\"" + (o == null ? "null" : o.ToString()) + "\"");

            //处理可空枚举类型
            if (nullableType != null)
            {
                ConstructorInfo c = typeof(T).GetConstructor(new Type[] { nullableType });
                return (T)c.Invoke(new object[] { o });
            }
            return (T)o;
        }

        /// <summary>
        /// 从字符串转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型（包括可空枚举）</typeparam>
        /// <param name="str">要转为枚举的字符串</param>
        /// <param name="defaultValue">转换失败时返回的默认值</param>
        /// <returns>转换结果</returns>
        public static T GetEnum<T>(string str, T defaultValue)
        {
            try
            {
                return GetEnum<T>(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 判断是否定义了FlagsAttribute属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasFlagsAttribute(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(FlagsAttribute), true);
            return attributes != null && attributes.Length > 0;
        }

        /// <summary>
        /// 判断是否包含指定的值
        /// </summary>
        /// <param name="multValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMarked(Enum multValue, Enum value)
        {
            return (Convert.ToInt32(multValue) & Convert.ToInt32(value)) == Convert.ToInt32(value);
        }

        /// <summary>
        /// 将指定的值拆分为一个枚举值的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T[] GetValues<T>(Enum values)
        {
            List<T> l = new List<T>();
            foreach (Enum v in Enum.GetValues(typeof(T)))
            {
                if (IsMarked(values, v))
                {
                    l.Add((T)((object)v));
                }
            }
            return l.ToArray();
        }

        /// <summary>
        /// 获取枚举值定义的属性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static EnumAttribute GetAttribute(Enum value)
        {
            if (enumAttr.ContainsKey(value))
            {
                EnumAttribute ea = enumAttr[value];
                return ea;
            }
            else
            {
                FieldInfo field = value.GetType().GetField(value.ToString());
                if (field == null) return null;
                EnumAttribute ea = null;
                object[] attributes = field.GetCustomAttributes(typeof(EnumAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    ea = (EnumAttribute)attributes[0];
                }
                enumAttr[value] = ea;
                return ea;
            }
        }
    }

    /// <summary>
    ///描述枚举的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumAttribute : Attribute
    {
        private string _name;
        private string _description;

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        public EnumAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        /// <param name="description">枚举描述</param>
        public EnumAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
