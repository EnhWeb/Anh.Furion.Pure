// MIT License
//
// Copyright (c) 2021-present 闪住优美, szzzqt.com Co.,Ltd and Contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anh.Furion.Pure.AnhRemoteRequest;
/// <summary>
/// 方法参数信息
/// </summary>
internal class MethodParameterInfo
{
	/// <summary>
	/// 参数
	/// </summary>
	internal ParameterInfo Parameter { get; set; }

	/// <summary>
	/// 参数名
	/// </summary>
	internal string Name { get; set; }

	/// <summary>
	/// 参数值
	/// </summary>
	internal object Value { get; set; }
}