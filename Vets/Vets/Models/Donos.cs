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
        public string Nome { get; set; }

        /// <summary>
        /// Número do NIF
        /// </summary>
        public string NIF { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Animais> ListaAnimais { get; set; }
    }
}