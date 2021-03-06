﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("multicast_ports", Schema = "public")]
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("multicast_port_id", Order = 1)]
        public int Id { get; set; }

        [Column("multicast_port_number", Order = 2)]
        public int Number { get; set; }

       
    }
}