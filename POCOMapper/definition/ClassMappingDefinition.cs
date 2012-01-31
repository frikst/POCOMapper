using System;
using System.Collections.Generic;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common;
using POCOMapper.mapping.special;

namespace POCOMapper.definition
{
	/// <summary>
	/// Class mapping specification definition class.
	/// </summary>
	/// <typeparam name="TFrom">Source class.</typeparam>
	/// <typeparam name="TTo">Destination class.</typeparam>
	public class ClassMappingDefinition<TFrom, TTo> : IMappingDefinition
	{
		private readonly List<Tuple<Type, Type>> aSubClassMaps;
		private Type aMapping;
		private Action<TFrom, TTo> aPostprocessDelegate;

		#region Implementation of IMappingDefinition

		internal ClassMappingDefinition()
		{
			this.aSubClassMaps = new List<Tuple<Type, Type>>();
			this.aMapping = null;
			this.aPostprocessDelegate = null;
		}

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			IMapping<TFrom, TTo> mapping;
			if (this.aMapping == null)
			{
				mapping = new ObjectToObject<TFrom, TTo>(allMappings);

				if (aSubClassMaps.Count > 0)
					mapping = new SubClassToObject<TFrom, TTo>(allMappings, aSubClassMaps, mapping);
			}
			else
			{
				mapping = (IMapping<TFrom, TTo>)Activator.CreateInstance(this.aMapping, allMappings);
			}

			if (this.aPostprocessDelegate != null)
				mapping = new Postprocess<TFrom, TTo>(mapping, this.aPostprocessDelegate);
			return mapping;
		}

		Type IMappingDefinition.From
		{
			get { return typeof(TFrom); }
		}

		Type IMappingDefinition.To
		{
			get { return typeof(TTo); }
		}

		MappingType IMappingDefinition.Type
		{
			get { return MappingType.ClassMapping; }
		}

		#endregion

		/// <summary>
		/// Adds subclass mapping.
		/// </summary>
		/// <typeparam name="TSubFrom">Source subclass.</typeparam>
		/// <typeparam name="TSubTo">Destination subclass.</typeparam>
		/// <returns>The class definition specification object.</returns>
		public ClassMappingDefinition<TFrom, TTo> MapSubClass<TSubFrom, TSubTo>()
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
		public ClassMappingDefinition<TFrom, TTo> Postprocess(Action<TFrom, TTo> postprocessDelegate)
		{
			this.aPostprocessDelegate = postprocessDelegate;

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
			this.aMapping = typeof(TMapping);
		}
	}
}