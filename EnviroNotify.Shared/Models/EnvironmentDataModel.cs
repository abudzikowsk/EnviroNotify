using System.ComponentModel.DataAnnotations;

namespace EnviroNotify.Shared.Models;

public class EnvironmentDataModel
{
    [Required]
    public double Humidity { get; set; }
    
    [Required]
    public double Temperature { get; set; }
}