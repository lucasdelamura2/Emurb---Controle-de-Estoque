using System.ComponentModel.DataAnnotations;

namespace EmurbEstoque.Models.Validations
{
    public class DataMinimaAttribute : ValidationAttribute
    {
        private readonly DateTime _dataMinima;

        public DataMinimaAttribute(string dataMinima)
        {
            _dataMinima = DateTime.Parse(dataMinima);
            ErrorMessage = $"A data não pode ser anterior a {_dataMinima:01-01-2025}.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            DateTime dataValor;
            if (DateTime.TryParse(value.ToString(), out dataValor))
            {
                return dataValor.Date >= _dataMinima.Date;
            }
            return false;
        }
    }
}