using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.ApiViewModel
{
    public class PagingParameterModel
    {
        public PagingParameterModel()
        {
            pageNumber = 1;
            _pageSize = 10;
        }
        const int maxPageSize= 10;
        public int pageNumber { get; set; } 
        public int _pageSize { get; set; } 

        public int pageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize=(value>maxPageSize)?maxPageSize: value;
            }
        }
    }
}