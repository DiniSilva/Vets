using System.ComponentModel.DataAnnotations;

namespace Vets.Models{
    /// <summary>
    /// 
    /// </summary>
    public class Veterinarios{
        public Veterinarios(){
            ListaConsultas = new HashSet<Consultas>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Nº de cédulas profossionais
        /// </summary>
        [Display(Name ="Nº cédula profissional")]
        public string NumCedulaProf { get; set; }

        /// <summary>
        /// Nome do ficheiro que contêm fotografias
        /// </summary>
        public string Fotografia { get; set; }

        /// <summary>
        /// Numero de consultas feitas
        /// </summary>
        public ICollection<Consultas> ListaConsultas { get; set; }
    }
}