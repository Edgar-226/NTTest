namespace NTTest.Models
{
    public class Numbers
    {
        private HashSet<int> numeros;

        public Numbers()
        {
            numeros = new HashSet<int>(Enumerable.Range(1, 100));
        }

        public void Extract(int numero)
        {
            if (numero < 1 || numero > 100)
            {
                throw new ArgumentException("El número debe estar entre 1 y 100.");
            }

            numeros.Remove(numero);
        }

        public int CalcularNumeroFaltante()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (!numeros.Contains(i))
                {
                    return i;
                }
            }

            throw new InvalidOperationException("No se encontró ningún número faltante.");
        }
    }
}
