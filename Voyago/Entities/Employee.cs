using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Employee information such as salary, department, and title.
    /// </summary>
    public partial class Employee
    {
        public Employee()
        {
            EmployeeAddresses = new HashSet<EmployeeAddress>();
            EmployeeDepartmentHistories = new HashSet<EmployeeDepartmentHistory>();
            EmployeePayHistories = new HashSet<EmployeePayHistory>();
            InverseManager = new HashSet<Employee>();
            JobCandidates = new HashSet<JobCandidate>();
            PurchaseOrderHeaders = new HashSet<PurchaseOrderHeader>();
        }

        /// <summary>
        /// Primary key for Employee records.
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Unique national identification number such as a social security number.
        /// </summary>
        public string NationalIdnumber { get; set; } = null!;
        /// <summary>
        /// Identifies the employee in the Contact table. Foreign key to Contact.ContactID.
        /// </summary>
        public int ContactId { get; set; }
        /// <summary>
        /// Network login.
        /// </summary>
        public string LoginId { get; set; } = null!;
        /// <summary>
        /// Manager to whom the employee is assigned. Foreign Key to Employee.M
        /// </summary>
        public int? ManagerId { get; set; }
        /// <summary>
        /// Work title such as Buyer or Sales Representative.
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Date of birth.
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// M = Married, S = Single
        /// </summary>
        public string MaritalStatus { get; set; } = null!;
        /// <summary>
        /// M = Male, F = Female
        /// </summary>
        public string Gender { get; set; } = null!;
        /// <summary>
        /// Employee hired on this date.
        /// </summary>
        public DateTime HireDate { get; set; }
        /// <summary>
        /// Job classification. 0 = Hourly, not exempt from collective bargaining. 1 = Salaried, exempt from collective bargaining.
        /// </summary>
        public bool? SalariedFlag { get; set; }
        /// <summary>
        /// Number of available vacation hours.
        /// </summary>
        public short VacationHours { get; set; }
        /// <summary>
        /// Number of available sick leave hours.
        /// </summary>
        public short SickLeaveHours { get; set; }
        /// <summary>
        /// 0 = Inactive, 1 = Active
        /// </summary>
        public bool? CurrentFlag { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Contact Contact { get; set; } = null!;
        public virtual Employee? Manager { get; set; }
        public virtual SalesPerson SalesPerson { get; set; } = null!;
        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }
        public virtual ICollection<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
        public virtual ICollection<EmployeePayHistory> EmployeePayHistories { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }
        public virtual ICollection<JobCandidate> JobCandidates { get; set; }
        public virtual ICollection<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
    }
}
