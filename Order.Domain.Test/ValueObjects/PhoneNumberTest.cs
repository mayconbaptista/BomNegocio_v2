using Order.Domain.ValueObjects;

namespace Order.Domain.Test.ValueObjects
{
    public class PhoneNumberTest
    {
        [Fact(DisplayName = "Teste de criação de um numero de telefone válido com operador implicito")]
        public void Create_ValidPhoneNumber_UsingImplictOperator ()
        {
            PhoneNumber phoneNumber = "(34) 9 9905-8416";
            PhoneNumber phoneNumber1 = "(34) 9 99058416";
            PhoneNumber phoneNumber2 = "(34) 999058416";
            PhoneNumber phoneNumber3 = "(34)999058416";
            PhoneNumber phoneNumber4 = "(34999058416";
            PhoneNumber phoneNumber5 = "34)999058416";
            PhoneNumber phoneNumber6 = "34999058416";
        }

        [Fact(DisplayName = "Teste de criação de um numero de telefone válido com construtor")]
        public void Create_ValidPhoneNumber_Constructor()
        {
            PhoneNumber phoneNumber =  new PhoneNumber("(34) 9 9905-8416");
            PhoneNumber phoneNumber1 = new PhoneNumber("(34) 9 99058416");
            PhoneNumber phoneNumber2 = new PhoneNumber("(34) 999058416");
            PhoneNumber phoneNumber3 = new PhoneNumber("(34)999058416");
            PhoneNumber phoneNumber4 = new PhoneNumber("(34999058416");
            PhoneNumber phoneNumber5 = new PhoneNumber("34)999058416");
            PhoneNumber phoneNumber6 = new PhoneNumber("34999058416");
        }

        [Fact(DisplayName = "Teste de comparação de numeros de telefones iquais")]
        public void OperatorEquals_WithSucess()
        {
            PhoneNumber phoneNumber = "(34) 9 9905-8416";
            PhoneNumber phoneNumber1 = "(34) 9 99058416";
            PhoneNumber phoneNumber2 = "(34) 999058416";
            PhoneNumber phoneNumber3 = "(34)999058416";
            PhoneNumber phoneNumber4 = "(34999058416";
            PhoneNumber phoneNumber5 = "34)999058416";
            PhoneNumber phoneNumber6 = "34999058416";

            Assert.Equal(phoneNumber, phoneNumber1);
            Assert.Equal(phoneNumber1, phoneNumber2);
            Assert.Equal(phoneNumber2, phoneNumber3);
            Assert.Equal(phoneNumber3, phoneNumber4);
            Assert.Equal(phoneNumber4, phoneNumber5);
            Assert.Equal(phoneNumber5, phoneNumber6);
        }

        [Fact(DisplayName = "Teste de comparação de numeros de telefone diferentes")]
        public void OperatorEquals_WithNotSucess()
        {
            PhoneNumber phoneNumber = "(34) 9 8805-8416";
            PhoneNumber phoneNumber1 = "(34) 9 99058416";
            PhoneNumber phoneNumber2 = "(37) 999058416";
            PhoneNumber phoneNumber3 = "(34)999058415";
            PhoneNumber phoneNumber4 = "(34998058416";
            PhoneNumber phoneNumber5 = "34)999058416";
            PhoneNumber phoneNumber6 = "34999058410";

            Assert.NotEqual(phoneNumber, phoneNumber1);
            Assert.NotEqual(phoneNumber1, phoneNumber2);
            Assert.NotEqual(phoneNumber2, phoneNumber3);
            Assert.NotEqual(phoneNumber3, phoneNumber4);
            Assert.NotEqual(phoneNumber4, phoneNumber5);
            Assert.NotEqual(phoneNumber5, phoneNumber6);
        }
    }
}