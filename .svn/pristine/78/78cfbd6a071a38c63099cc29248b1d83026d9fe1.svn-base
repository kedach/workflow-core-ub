using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.UBWF.Context;
using WorkflowCore.UBWF.Models;
using WorkflowCore.UBWF.Models.TOA;
using WorkflowCore.UBWF.SimProvider;

namespace WorkflowCore.UBWF.Rules
{
    public class DestinationRule : IUBEventRule
    {
        public void Run(ProcInstContext PIContext)
        {

            SimTOAProvider simTOAProvider = new SimTOAProvider(PIContext);

            UBJob job = (UBJob)PIContext.GetValue("Job");

            DestinationResult destinationResult = simTOAProvider.GetDestinations(job);

            PIContext.SetValue("DestinationResult", destinationResult);
        }
    }
}
