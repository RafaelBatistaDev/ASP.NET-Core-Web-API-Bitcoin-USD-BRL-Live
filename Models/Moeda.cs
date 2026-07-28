using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models;

/// <summary>
/// Representa uma criptomoeda cadastrada no sistema
/// </summary>
public class Moeda
{
    /// <summary>Identificador único</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>Nome da criptomoeda (ex: Bitcoin)</summary>
    [Required(ErrorMessage = "O nome da moeda é obrigatório")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 50 caracteres")]
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>Símbolo da criptomoeda (ex: BTC)</summary>
    [Required(ErrorMessage = "O símbolo é obrigatório (ex: BTC)")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "O símbolo deve ter entre 1 e 10 caracteres")]
    [JsonPropertyName("simbolo")]
    public string Simbolo { get; set; } = string.Empty;

    /// <summary>Preço em USD</summary>
    [Required]
    [Range(0.00000001, 999_999_999.99999999, ErrorMessage = "O preço deve ser maior que zero")]
    [Column(TypeName = "decimal(18,8)")]
    [JsonPropertyName("preco")]
    public decimal Preco { get; set; }

    /// <summary>Data de cadastro</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    /// <summary>Data da última atualização</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime? DataAtualizacao { get; set; }
}
