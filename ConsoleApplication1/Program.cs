using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Saga;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var busConfig = new BusConfiguration();
            busConfig.UseTransport<SqlServerTransport>();
            busConfig.UsePersistence<NHibernatePersistence>();
            busConfig.Transactions().DisableDistributedTransactions();

            var number = 1;
            using (var bus = Bus.Create(busConfig).Start())
            {
                while (true)
                {
                    Console.ReadLine();
                    bus.SendLocal(new TestMessage()
                    {
                        Number = number
                    });
                    number++;
                }
            }
        }
    }

    public class TestMessage : IMessage
    {
        public int Number { get; set; }
    }

    public class TestSaga : Saga<TestSagaData>,
        IAmStartedByMessages<TestMessage>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
        {
        }

        public void Handle(TestMessage message)
        {
            Console.WriteLine("Beginning processing {0}", message.Number);
            Thread.Sleep(5000);
            Console.WriteLine("Finishing processing {0}", message.Number);
        }
    }

    public class TestSagaData : ContainSagaData
    {
    }
}
