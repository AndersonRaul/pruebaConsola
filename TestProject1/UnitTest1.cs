using NUnit.Framework;
using preba.Entidades;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //arrange setear variables que se van a utilizar en la prueba
            int valor1 = 10;
            int valor2 = 20;
            int esperado = 30;
            Operaciones operaciones = new Operaciones();
            //act donde se va a ralizar el procedimient
            var result = operaciones.Sumar(valor1, valor2);
            //Assert
            Assert.AreEqual(esperado, result);    
        }
    }
}