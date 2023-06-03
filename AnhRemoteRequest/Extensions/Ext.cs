// MIT License
//
// Copyright (c) 2021-present 闪住优美, szzzqt.com Co.,Ltd and Contributors

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Internal;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anh.Furion.Pure.AnhRemoteRequest;
internal static class Ext
{
	/// <summary>
	/// 判断是否是富基元类型
	/// </summary>
	/// <param name="type">类型</param>
	/// <returns></returns>
	internal static bool IsRichPrimitive(this Type type)
	{
		// 处理元组类型
		if (type.IsValueTuple()) return false;

		// 处理数组类型，基元数组类型也可以是基元类型
		if (type.IsArray) return type.GetElementType().IsRichPrimitive();

		// 基元类型或值类型或字符串类型
		if (type.IsPrimitive || type.IsValueType || type == typeof(string)) return true;

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) return type.GenericTypeArguments[0].IsRichPrimitive();

		return false;
	}

	/// <summary>
	/// 查找方法指定特性，如果没找到则继续查找声明类
	/// </summary>
	/// <typeparam name="TAttribute"></typeparam>
	/// <param name="method"></param>
	/// <param name="inherit"></param>
	/// <returns></returns>
	internal static TAttribute GetFoundAttribute<TAttribute>(this MethodInfo method, bool inherit)
		where TAttribute : Attribute
	{
		// 获取方法所在类型
		var declaringType = method.DeclaringType;

		var attributeType = typeof(TAttribute);

		// 判断方法是否定义指定特性，如果没有再查找声明类
		var foundAttribute = method.IsDefined(attributeType, inherit)
			? method.GetCustomAttribute<TAttribute>(inherit)
			: (
				declaringType.IsDefined(attributeType, inherit)
				? declaringType.GetCustomAttribute<TAttribute>(inherit)
				: default
			);

		return foundAttribute;
	}


	/// <summary>
	/// 合并两个字典
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="dic">字典</param>
	/// <param name="newDic">新字典</param>
	/// <returns></returns>
	internal static Dictionary<string, T> AddOrUpdate<T>(this Dictionary<string, T> dic, IDictionary<string, T> newDic)
	{
		foreach (var key in newDic.Keys)
		{
			if (dic.TryGetValue(key, out var value))
			{
				dic[key] = value;
			}
			else
			{
				dic.Add(key, newDic[key]);
			}
		}

		return dic;
	}

	/// <summary>
	/// 合并两个字典
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="dic">字典</param>
	/// <param name="newDic">新字典</param>
	internal static void AddOrUpdate<T>(this ConcurrentDictionary<string, T> dic, Dictionary<string, T> newDic)
	{
		foreach (var (key, value) in newDic)
		{
			dic.AddOrUpdate(key, value, (key, old) => value);
		}
	}
}
