using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessObject.ViewModel
{
    public class TaskVerificationList
    {
      
            public TaskVerificationList()
            {
                this.VerifierList = new List<TaskVerificationList>();
            }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public bool IsMultiVerifier { get; set; }
            public bool IsSelected { get; set; }
            public string Message { get; set; }
            public bool IsExceedAmount { get; set; }
            public int EventId { get; set; }
            public List<TaskVerificationList> VerifierList { get; set; }
            // public List<Task> TaskList { get; set; }
        
    }
}
