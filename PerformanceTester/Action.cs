using PerformanceTesting.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static PerformanceTester.Helper;

namespace PerformanceTester
{
    public class Action
    {
        
        public Action(Methods method, string target, bool addRequestDto = false, string requestBody = "")
        {
            Method = method;
            Target = target;
            RequestBody = requestBody;
            RequestDto = null;
            if (addRequestDto)
                AddRequestDto();
        }

        public Methods Method { get; set; }
        public string Target { get; set; }
        public string RequestBody { get; set; }
        public FilterDto? RequestDto { get; set; }
               
        public void AddRequestDto()
        {
            RequestDto = new FilterDto() 
            { 
                Countries = new List<string> { "Germany", "USA", "France" }
            };
        }
    }
}
