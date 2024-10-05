using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyTourChoice.API.Application.Models.BaseModels;

namespace EasyTourChoice.API.Domain;

public class Location : LocationBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LocationId { get; set; }
}