﻿using System;
using System.Linq;
using System.Collections.Generic;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common;
using POCOMapper.mapping.common.parser;
using POCOMapper.mapping.special;

namespace POCOMapper.definition
{
	/// <summary>
	/// Class mapping specification definition class.
	/// </summary>
	/// <typeparam name="TFrom">Source class.</typeparam>
	/// <typeparam name="TTo">Destination class.</typeparam>
	public class ExactMappingDefinition<TFrom, TTo> : IMappingDefinition
	{
		private readonly List<Tuple<Type, Type>> aSubClassMaps;
		private Type aMapping;
		private Action<TFrom, TTo> aPostprocessDelegate;
		private bool aUseImplicitMappings;
		private readonly List<IMemberMappingDefinition> aExplicitMappings;
		private Func<TFrom, TTo> aFactoryFunction;
		private Func<TFrom, TTo> aMappingFunc;
		private Action<TFrom, TTo> aMappingAction;

		internal ExactMappingDefinition()
		{
			this.aSubClassMaps = new List<Tuple<Type, Type>>();
			this.aMapping = null;
			this.aPostprocessDelegate = null;
			this.aUseImplicitMappings = true;
			this.aExplicitMappings = new List<IMemberMappingDefinition>();
			this.aFactoryFunction = null;
			this.aMappingAction = null;
			this.aMappingFunc = null;
		}

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			IMapping<TFrom, TTo> mapping;
			if (this.aMapping != null)
			{
				mapping = (IMapping<TFrom, TTo>)Activator.CreateInstance(this.aMapping, allMappings);
			}
			else if (this.aMappingFunc != null)
			{
				mapping = new FuncMapping<TFrom, TTo>(this.aMappingFunc);
			}
			else if (this.aMappingAction != null)
			{
				mapping = new FuncMapping<TFrom, TTo>(this.aMappingAction);
			}
			else
			{
				IEnumerable<PairedMembers> members = this.aExplicitMappings.Select(x => x.CreateMapping(allMappings));

				mapping = new ObjectToObject<TFrom, TTo>(aFactoryFunction, allMappings, members, this.aUseImplicitMappings);

				if (aSubClassMaps.Count > 0)
					mapping = new SubClassToObject<TFrom, TTo>(allMappings, aSubClassMaps, mapping);
			}

			if (this.aPostprocessDelegate != null)
				mapping = new Postprocess<TFrom, TTo>(mapping, this.aPostprocessDelegate);
			return mapping;
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

		#endregion

		/// <summary>
		/// Adds subclass mapping.
		/// </summary>
		/// <typeparam name="TSubFrom">Source subclass.</typeparam>
		/// <typeparam name="TSubTo">Destination subclass.</typeparam>
		/// <returns>The class definition specification object.</returns>
		public ExactMappingDefinition<TFrom, TTo> MapSubClass<TSubFrom, TSubTo>()
			where TSubFrom : TFrom
			where TSubTo : TTo
		{
			this.aSubClassMaps.Add(new Tuple<Type, Type>(typeof(TSubFrom), typeof(TSubTo)));

			return this;
		}

		/// <summary>
		/// Adds the entity postprocess delegate. Delegate is called for each entity pair after
		/// the mapping process is complete.
		/// </summary>
		/// <param name="postprocessDelegate">The delegate which should be called after the mapping process.</param>
		/// <returns>The class definition specification object.</returns>
		public ExactMappingDefinition<TFrom, TTo> Postprocess(Action<TFrom, TTo> postprocessDelegate)
		{
			this.aPostprocessDelegate = postprocessDelegate;

			return this;
		}

		public ExactMappingDefinition<TFrom, TTo> Factory(Func<TFrom, TTo> factoryFunction)
		{
			this.aFactoryFunction = factoryFunction;

			return this;
		}

		/// <summary>
		/// Mapping class specified by the <typeparamref name="TMapping"/> should be used for
		/// mapping the <typeparamref name="TFrom"/> class to the <typeparamref name="TTo"/> class.
		/// </summary>
		/// <typeparam name="TMapping"></typeparam>
		public void Using<TMapping>()
			where TMapping : IMapping<TFrom, TTo>
		{
			if (this.aExplicitMappings.Count > 0 || this.aSubClassMaps.Count > 0 || !this.aUseImplicitMappings)
				throw new InvalidMapping("Cannot map using custom mappings when some mapping settings are in use");
			this.aMapping = typeof(TMapping);
		}

		public void Using(Func<TFrom, TTo> mappingFunc)
		{
			this.aMappingFunc = mappingFunc;
		}

		public void Using(Action<TFrom, TTo> mappingAction)
		{
			this.aMappingAction = mappingAction;
		}

		/// <summary>
		/// Marks the mapping to use only explicit column mapping.
		/// </summary>
		public ExactMappingDefinition<TFrom, TTo> OnlyExplicit
		{
			get
			{
				this.aUseImplicitMappings = false;
				return this;
			}
		}

		public ExactMappingDefinition<TFrom, TTo> Member(string from, string to)
		{
			SimpleMemberMappingDefinition def = new SimpleMemberMappingDefinition(typeof(TFrom), typeof(TTo), from, to);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ExactMappingDefinition<TFrom, TTo> Member<TFromType, TToType>(string from, string to, Action<MemberMappingDefinition<TFromType, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TToType> def = new MemberMappingDefinition<TFromType, TToType>(typeof(TFrom), typeof(TTo), from, to);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ExactMappingDefinition<TFrom, TTo> MemberFrom<TFromType>(string from, Action<MemberMappingDefinition<TFromType, TTo>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TTo> def = new MemberMappingDefinition<TFromType, TTo>(typeof(TFrom), typeof(TTo), from, null);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ExactMappingDefinition<TFrom, TTo> MemberTo<TToType>(string to, Action<MemberMappingDefinition<TFrom, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFrom, TToType> def = new MemberMappingDefinition<TFrom, TToType>(typeof(TFrom), typeof(TTo), null, to);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}
	}
}