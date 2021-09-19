using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class RabbitMQCode : Form
    {
        public RabbitMQCode()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TaskDescription.Visible = false;
            TaskDescriptionLabel.Visible = false;
            SendRequest.Visible = false;
            Receipt.Visible = false;

            ServerResp.Visible = false;
            ServerRespLabel.Visible = false;
            ListButton.Visible = false;

            getButton.Visible = false;
            blockingCheck.Visible = false;
        }

        private void SendRequest_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("Send button clicked");

            String idstring = TaskId.Text.ToString();
            int id = 0;

            if (TaskOper.SelectedItem.ToString() == "List")
            {                                
                if (!(idstring.ToLower() == "all") && !(int.TryParse(idstring, out id)))
                {
                    MessageBox.Show("Invalid input: " + idstring);
                    return;
                }

                ListTask(idstring);
                return;
            }


                if (!(int.TryParse(idstring, out id)))
            {
                MessageBox.Show("Invalid input: " + idstring);
                return;
            }

            if (id <= 0)
            {
                MessageBox.Show("Invalid task id " + idstring);
                return;
            }

            String message = TaskOper.SelectedItem.ToString();
            message += ";" + idstring;

            if (TaskOper.SelectedItem.ToString() == "Add")
            {
                String description = TaskDescription.Text;

                if (description == null || description == "")
                {
                    MessageBox.Show("Description required");
                    return;
                }

                message += ";" + description;
            }
            QueueMsg(message);
            System.Threading.Thread.Sleep(3000);
            ListenForResponse();
        }

        private void Receive_Click(object sender, EventArgs e)
        {
            // Add code to listen for message
            
            String idstring = TaskId.Text.ToString();

            int id = 0;

            if (!(idstring.ToLower() == "all") && !(int.TryParse(idstring, out id)))
            {
                MessageBox.Show("Invalid input: " + idstring);
                return;
            }

            ListTask(idstring);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void TaskOper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TaskOper.SelectedItem.ToString() == "Add")
            {
                TaskDescription.Visible = true;
                TaskDescriptionLabel.Visible = true;
                SendRequest.Visible = true;
                Receipt.Visible = true;
            }
            else
            {
                TaskDescription.Visible = false;
                TaskDescriptionLabel.Visible = false;
                SendRequest.Visible = true;
                Receipt.Visible = true;
            }
        }


        private void ListTask(String id)
        {
            String msg = "List;" + id;

            ServerResp.Text = "";

            QueueMsg(msg);
            System.Threading.Thread.Sleep(3000);
            ListenForResponse();
        }


        private void ModeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ModeSelector.SelectedItem.ToString() == "Send")
            {
                TaskDescription.Visible = false;
                TaskDescriptionLabel.Visible = false;
                SendRequest.Visible = true;
                Receipt.Visible = true;

                ServerResp.Visible = true;
                ServerRespLabel.Visible = true;
                ListButton.Visible = true;

                getButton.Visible = false;
                blockingCheck.Visible = false;
            }
            else
            { // Listen mode
                TaskDescription.Visible = false;
                TaskDescriptionLabel.Visible = false;
                SendRequest.Visible = false;
                Receipt.Visible = false;

                ServerResp.Visible = true;
                ServerRespLabel.Visible = true;
                ListButton.Visible = false;

                getButton.Visible = true;
                blockingCheck.Visible = true;
            }

        }

        private void QueueMsg(String msg)
        {
            bool receipt_required = Receipt.Checked;
            String cloudAmqs = Environment.GetEnvironmentVariable("CLOUDAMQP_URL");

            if (cloudAmqs == null)
            {
                MessageBox.Show("Environment variable CLOUDAMQP_URL is not set");
                return;
            }

            
            try
            {
                var factory = new RabbitMQ.Client.ConnectionFactory { Uri = new Uri(cloudAmqs) };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(msg);

                    String routing_key = "post.taskdb.test";

                    channel.ConfirmSelect();

                    channel.ExchangeDeclare("task_exch", "topic", false, false, null);
                    
                    var result = channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                   
                    RabbitMQ.Client.IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.Expiration = "5000"; // Message will be deleted from queue after 5 seconds

                    channel.BasicPublish(exchange: "task_exch",
                                     routingKey: routing_key,
                                     basicProperties: props,
                                     body: body,
                                     mandatory: receipt_required);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message + e.GetType());
            }
            finally
            {
                // Should close channel and connection here, but they are out of scope
                // channel.Close();
                // connection.Close();
            }                
        }

        private void ListenForResponse()
        {
            const String queueName = "task_response";
            String cloudAmqs = Environment.GetEnvironmentVariable("CLOUDAMQP_URL");

            try
            {
                var factory = new RabbitMQ.Client.ConnectionFactory { Uri = new Uri(cloudAmqs) };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(queueName, "direct", false, false, null);
                    channel.QueueDeclare(queueName,true,false,false,null);

                    var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        processResponse(message);                    
                    };

                   channel.QueueBind(queue: queueName,
                   exchange: queueName,
                   routingKey: queueName, null);

                   channel.BasicConsume(queueName,true, "", false, false, null, consumer);
                    
                }
            }
            catch (Exception e) // RabbitMQ.Client.Exceptions. e)
            {
                MessageBox.Show("Exception: " + e.Message + e.GetType());
            }
            finally
            {
                // Should close channel and connection here, but they are out of scope
                // channel.Close();
                // connection.Close();
                
            }
         }
        private void processResponse(String msg)
        {
            var myList = msg.Split(':');

            ServerResp.Text = "";
            foreach(var a in myList)
            {
                var tmp = a.Split(';');
                ServerResp.Text += "Task id: " + tmp[0] + "\r\n";
                ServerResp.Text += "\t Task description: " + tmp[1] + "\r\n";
                ServerResp.Text += "\t Task completed status: " + tmp[2] + "\r\n";
                ServerResp.Text += "\r\n";
            }            
        }

        private void getButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Later we will get something from a python/node.js/other language server");
        }
    }
}