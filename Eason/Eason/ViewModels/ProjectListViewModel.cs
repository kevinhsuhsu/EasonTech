using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Eason.Models;
using System.Web.Mvc;


namespace Eason.ViewModels
{
    public class ProjectListViewModel
    {
        public ProjectList ProjectList { get; set; }
        public ProjectListSearchModel SearchParameter { get; set; }
        public List<SelectListItem> PJT_PARTICIPANT { get; set; }
        public List<SelectListItem> PJT_TYPE { get; set; }
        public List<SelectListItem> PJT_CUSTOMER { get; set; }
    }
}