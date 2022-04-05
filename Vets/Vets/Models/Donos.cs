using System.ComponentModel.DataAnnotations;

namespace Vets.Models{
    public class Donos
    {
        public Donos(){
            ListaAnimais = new HashSet<Animais>();
        }

        /// <summary>
        /// PK para a tabela dos Donos
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome o Dono do animal
        /// </summary>
        [Required(ErrorMessage="Prenchimento Obrigatório")]
        [StringLength(20, ErrorMessage ="O {@} não pode ter mais do que 20 caracteres.")]
        [RegularExpression("[A-ZÂÓ-azáéíóúàèìòùâêîôûãõäëïöüÿñç]+" ,ErrorMessage ="No {@} são só aceites letras")]
        public string Nome { get; set; }

        /// <summary>
        /// Número do NIF do dono do animal
        /// </summary>
        [Required(ErrorMessage = "Prenchimento Obrigatório")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O {@} tem de ter {l} caracteres.")]
        [RegularExpression("[123578] + [0-9]{9}", ErrorMessage = "")]
        public string NIF { get; set; }

        /// <summary>
        /// Sexo do dono
        /// Ff - feminino e Mm - masculino
        /// </summary>
        [Required(ErrorMessage = "Prenchimento Obrigatório")]
        [RegularExpression("[FfMm]", ErrorMessage = "No {@} só se aceitam as letras F ou M.")]
        public string Sexo { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress(ErrorMessage ="Introduza o email correto")]
        public string Email { get; set; }

        /// <summary>
        /// Lista dos animais de quem o Dono é dono
        /// </summary>
        public ICollection<Animais> ListaAnimais { get; set; }
    }
}