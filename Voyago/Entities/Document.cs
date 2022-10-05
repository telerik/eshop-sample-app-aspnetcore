using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Product maintenance documents.
    /// </summary>
    public partial class Document
    {
        public Document()
        {
            ProductDocuments = new HashSet<ProductDocument>();
        }

        /// <summary>
        /// Primary key for Document records.
        /// </summary>
        public int DocumentId { get; set; }
        /// <summary>
        /// Title of the document.
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Directory path and file name of the document
        /// </summary>
        public string FileName { get; set; } = null!;
        /// <summary>
        /// File extension indicating the document type. For example, .doc or .txt.
        /// </summary>
        public string FileExtension { get; set; } = null!;
        /// <summary>
        /// Revision number of the document. 
        /// </summary>
        public string Revision { get; set; } = null!;
        /// <summary>
        /// Engineering change approval number.
        /// </summary>
        public int ChangeNumber { get; set; }
        /// <summary>
        /// 1 = Pending approval, 2 = Approved, 3 = Obsolete
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// Document abstract.
        /// </summary>
        public string? DocumentSummary { get; set; }
        /// <summary>
        /// Complete document.
        /// </summary>
        public byte[]? Document1 { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductDocument> ProductDocuments { get; set; }
    }
}
