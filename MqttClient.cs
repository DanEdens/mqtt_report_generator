using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System.Threading.Tasks;

namespace mqtt_report_generator
{
    public class MqttClient
    {
        private IManagedMqttClient managedMqttClient;

        public MqttClient(string brokerAddress, int brokerPort)
        {
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithTcpServer(brokerAddress, brokerPort)
                    .Build())
                .Build();

            managedMqttClient = new MqttFactory().CreateManagedMqttClient();
            Connect().GetAwaiter().GetResult();
        }

        public static async Task Connect()
        {
            /*
             * This sample creates a simple managed MQTT client and connects to a public broker.
             *
             * The managed client extends the existing _MqttClient_. It adds the following features.
             * - Reconnecting when connection is lost.
             * - Storing pending messages in an internal queue so that an enqueue is possible while the client remains not connected.
             */

            var mqttFactory = new MqttFactory();

            using (var managedMqttClient = mqttFactory.CreateManagedMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.hivemq.com")
                    .Build();

                var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                    .WithClientOptions(mqttClientOptions)
                    .Build();

                await managedMqttClient.StartAsync(managedMqttClientOptions);

                // The application message is not sent. It is stored in an internal queue and
                // will be sent when the client is connected.
                await managedMqttClient.EnqueueAsync("Topic", "Payload");

                Console.WriteLine("The managed MQTT client is connected.");

                // Wait until the queue is fully processed.
                SpinWait.SpinUntil(() => managedMqttClient.PendingApplicationMessagesCount == 0, 10000);

                Console.WriteLine($"Pending messages = {managedMqttClient.PendingApplicationMessagesCount}");
            }
        }

        public async Task Disconnect()
        {
            await managedMqttClient.StopAsync();
        }

        public async Task Publish(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();
            await managedMqttClient.EnqueueAsync(message);
        }

        public async Task Subscribe(string topic)
        {
            await managedMqttClient.SubscribeAsync(topic);
        }

        public async Task Unsubscribe(string topic)
        {
            await managedMqttClient.UnsubscribeAsync(topic);
        }

        public async Task<MqttApplicationMessage> RetrieveMessage(string topic)
        {
            // TODO: Implement message retrieval logic using the MQTT client library
            // Return the retrieved message
            return null;
        }
    }
}
