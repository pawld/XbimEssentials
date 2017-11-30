using System;
using log4net;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.ProfilePropertyResource;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc2x3.Kernel
{
	public partial class IfcRelAssociates : IExpressValidatable
	{
		public enum IfcRelAssociatesClause
		{
			WR21,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcRelAssociatesClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcRelAssociatesClause.WR21:
						retVal = Functions.SIZEOF(RelatedObjects.Where(temp => !((Functions.TYPEOF(temp).Contains("IFC2X3.IFCOBJECTDEFINITION")) || (Functions.TYPEOF(temp).Contains("IFC2X3.IFCPROPERTYDEFINITION"))))) == 0;
						break;
				}
			} catch (Exception ex) {
				var Log = LogManager.GetLogger("Xbim.Ifc2x3.Kernel.IfcRelAssociates");
				Log.Error(string.Format("Exception thrown evaluating where-clause 'IfcRelAssociates.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcRelAssociatesClause.WR21))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcRelAssociates.WR21", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}