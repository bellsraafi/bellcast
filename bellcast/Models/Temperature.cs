//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bellcast.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Temperature
    {
        public double min { get; set; }
        public double max { get; set; }
        public double day { get; set; }
        public double night { get; set; }
        public double morn { get; set; }
        public double eve { get; set; }
        public int dateTime { get; set; }
        public int cityId { get; set; }
    }
}
