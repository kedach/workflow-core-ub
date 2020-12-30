using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Context
{
    public class UBContext
    {
        public string WorkflowId { get; set; }
        IList<string> destinations = new List<string>();
        public IList<string> Destinations
        {
            get
            {
                return destinations;
            }
        }
        public string Destination { get; set; }
    }
}