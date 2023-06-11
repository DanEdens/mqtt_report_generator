using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

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
            managedMqttClient.StartAsync(options).GetAwaiter().GetResult();
        }

        public void Publish(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            managedMqttClient.PublishAsync(message).GetAwaiter().GetResult();
        }

        public void RetainedPublish(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithRetainFlag()
                .Build();

            managedMqttClient.PublishAsync(message).GetAwaiter().GetResult();
        }

        public void Subscribe(string topic)
        {
            managedMqttClient.SubscribeAsync(topic).GetAwaiter().GetResult();
        }

        public void Connect()
        {
            managedMqttClient.StartAsync().GetAwaiter().GetResult();
        }

        public void Disconnect()
        {
            managedMqttClient.StopAsync().GetAwaiter().GetResult();
        }
    }
}
