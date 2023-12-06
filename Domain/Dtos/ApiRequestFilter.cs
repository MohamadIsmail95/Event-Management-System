using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ApiRequestFilter
    {
       private int initPageNum;   
       private int initpageSize;
       public string? searchQuery { get;set; }
       public string? sortingField { get;set; }
       public string? sortingDir { get; set; }
       public int pageNumber { get { return initPageNum; } set { initPageNum = (value<=0) ? initPageNum = 1:value; } }
       public int pageSize { get { return initpageSize; } set { initpageSize = (value<=0) ? initpageSize=10:value; } }
        
       public  List<FilterList> ? filterLists { get; set; }
       
       
    }
    public class FilterList
    {
        public string key { get; set; }
        public List<string> values { get; set; }
    }
}
