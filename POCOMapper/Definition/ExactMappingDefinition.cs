﻿using System;
using KST.POCOMapper.mapping.@base;
using KST.POCOMapper.mapping.common;

namespace KST.POCOMapper.definition
{
	/// <summary>
	/// Class mapping specification definition class.
	/// </summary>
	/// <typeparam name="TFrom">Source class.</typeparam>
	/// <typeparam name="TTo">Destination class.</typeparam>
	public class ExactMappingDefinition<TFrom, TTo> : IMappingDefinition, IRulesDefinition<TFrom, TTo>
	{
		private int aPriority;
		private IMappingRules<TFrom, TTo> aRules;

		internal ExactMappingDefinition()
		{
			this.aPriority = 0;
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
		}

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			return this.aRules.Create(allMappings);
		}

		bool IMappingDefinition.IsFrom(Type from)
		{
			return from == typeof(TFrom);
		}

		bool IMappingDefinition.IsTo(Type to)
		{
			return to == typeof(TTo);
		}

		Tuple<Type, Type> IMappingDefinition.GetKey()
		{
			return new Tuple<Type, Type>(typeof(TFrom), typeof(TTo));
		}

		int IMappingDefinition.Priority
		{
			get { return this.aPriority; }
		}

		#endregion

		public ExactMappingDefinition<TFrom, TTo> SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		#region Implementation of IRulesDefinition<TFrom,TTo>

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules<TFrom, TTo>, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion
	}
}