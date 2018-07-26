using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Mapping.Decorators
{
	public class NullableRules<TFrom, TTo> : IMappingRules<TFrom, TTo>, IRulesDefinition<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		private IMappingRules<TFrom, TTo> aRules;

		public NullableRules()
		{
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
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

		#region Implementation of IMappingRules<TFrom,TTo>

		public IMapping<TFrom, TTo> Create(MappingDefinitionInformation mappingDefinition)
		{
			var underlayingMapping = this.aRules.Create(mappingDefinition);

			switch (underlayingMapping)
			{
				case IDirectMapping _:
					return underlayingMapping;
				case IMappingWithSyncSupport<TFrom, TTo> mappingWithSync:
					return new NullableWithSync<TFrom, TTo>(mappingWithSync);
				default:
					return new NullableWithMap<TFrom, TTo>(underlayingMapping);
			}
		}

		#endregion
	}
}
