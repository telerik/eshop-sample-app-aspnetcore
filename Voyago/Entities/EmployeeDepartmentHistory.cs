using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Employee department transfers.
    /// </summary>
    public partial class EmployeeDepartmentHistory
    {
        /// <summary>
        /// Employee identification number. Foreign key to Employee.EmployeeID.
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Department in which the employee worked including currently. Foreign key to Department.DepartmentID.
        /// </summary>
        public short DepartmentId { get; set; }
        /// <summary>
        /// Identifies which 8-hour shift the employee works. Foreign key to Shift.Shift.ID.
        /// </summary>
        public byte ShiftId { get; set; }
        /// <summary>
        /// Date the employee started work in the department.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Date the employee left the department. NULL = Current department.
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Shift Shift { get; set; } = null!;
    }
}
