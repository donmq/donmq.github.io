using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class eTM_Dept_Classification
    {
        [Key]
        [Column(Order = 0)]
        public string Class_Kind { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Dept_ID { get; set; }
        public string Dept_Name {get;set;}
        public string Class_Name { get; set; }
        public string Insert_By { get; set; }
        public DateTime Insert_At { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
    }

    public class VFactoryIndexOC
  {
    public int Id { get; set; }
    public int Level { get; set; }
    public int? ParentID { get; set; }
    public string Dept_ID { get; set; }   // Factory,ProdDept,Group,Cell
    public string Dept_Name { get; set; }
    public string Class_Name { get; set; }
    public int? SortSeq { get; set; }      // Sort sequence by Parent Id
    public string LineNum { get; set; }      // The first LineNum of Child level
    public int? RowCount { get; set; }
  }
}