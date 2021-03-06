﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resourceEdge.Domain.Entities
{
    [MetadataType(typeof(IDentityViewModel))]
    public class IdentityCodes
    {
        [Key]
        public int codeId { get; set; }
        public string employee_code { get; set; }
        public string backgroundagency_code { get; set; }
        public string vendors_code { get; set; }
        public string staffing_code { get; set; }
        public string users_code { get; set; }
        public string requisition_code { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> createddate { get; set; }
        public Nullable<int> modifiedBy { get; set; }
        public Nullable<System.DateTime> modifieddate { get; set; }
    }
}
