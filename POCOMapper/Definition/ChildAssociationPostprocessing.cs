using System;

namespace POCOMapper.definition
{
	public class ChildAssociationPostprocessing<TParent, TChild> : IChildAssociationPostprocessing
	{
		private Action<TParent, TChild> aPostprocessDelegate;

		#region Implementation of IChildAssociationPostprocessing

		Type IChildAssociationPostprocessing.Parent
		{
			get { return typeof(TParent); }
		}

		Type IChildAssociationPostprocessing.Child
		{
			get { return typeof(TChild); }
		}

		Delegate IChildAssociationPostprocessing.PostprocessDelegate
		{
			get
			{
				return this.aPostprocessDelegate;
			}
		}

		#endregion

		public ChildAssociationPostprocessing<TParent, TChild> Postprocess(Action<TParent, TChild> postprocessDelegate)
		{
			this.aPostprocessDelegate = postprocessDelegate;
			return this;
		}
	}
}