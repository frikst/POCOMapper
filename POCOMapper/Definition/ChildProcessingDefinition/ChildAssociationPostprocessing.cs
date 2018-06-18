using System;

namespace KST.POCOMapper.Definition.ChildProcessingDefinition
{
	public class ChildAssociationPostprocessing<TParent, TChild> : IChildAssociationPostprocessing
	{
		private Action<TParent, TChild> aPostprocessDelegate;

		#region Implementation of IChildAssociationPostprocessing

		Type IChildAssociationPostprocessing.Parent
			=> typeof(TParent);

		Type IChildAssociationPostprocessing.Child
			=> typeof(TChild);

		Delegate IChildAssociationPostprocessing.PostprocessDelegate
			=> this.aPostprocessDelegate;

		#endregion

		public ChildAssociationPostprocessing<TParent, TChild> Postprocess(Action<TParent, TChild> postprocessDelegate)
		{
			this.aPostprocessDelegate = postprocessDelegate;
			return this;
		}
	}
}