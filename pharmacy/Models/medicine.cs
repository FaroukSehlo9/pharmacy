﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pharmacy.Models
{
    [Table("medicine")]
    public partial class medicine
    {
        public medicine()
        {
            comments = new HashSet<comment>();
            orders = new HashSet<order>();
        }

        [Key]
        public int med_id { get; set; }
        [StringLength(50)]
        public string med_name { get; set; }
        public string med_desc { get; set; }
        [StringLength(150)]
        public string med_img { get; set; }
        public int? med_money { get; set; }
        public int? med_quantity { get; set; }
        [Column(TypeName = "date")]
        public DateTime? med_time { get; set; }
        [Column(TypeName = "date")]
        public DateTime? med_expiredate { get; set; }
        public int? user_id { get; set; }
        public int? cat_id { get; set; }

        [ForeignKey("cat_id")]
        [InverseProperty("medicines")]
        public virtual catalog cat { get; set; }
        [ForeignKey("user_id")]
        [InverseProperty("medicines")]
        public virtual user user { get; set; }
        [InverseProperty("med")]
        public virtual ICollection<comment> comments { get; set; }
        [InverseProperty("med")]
        public virtual ICollection<order> orders { get; set; }
    }
}