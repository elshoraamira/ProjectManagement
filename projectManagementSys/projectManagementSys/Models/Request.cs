//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace projectManagementSys.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        public int Id { get; set; }
        public int PM_ID { get; set; }
        public int Project_ID { get; set; }
        public int R_status { get; set; }
    
        public virtual project project { get; set; }
        public virtual user user { get; set; }
    }
}