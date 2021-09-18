import os, pika

def sendResponse(results, queue_name):
    
    url = os.getenv('CLOUDAMQP_URL')

    if not url:
        print("Environment variable CLOUDAMQP_URL not set")
        exit(-1)

    try:
        params = pika.URLParameters(url)
        params.socket_timeout = 5
        connection = pika.BlockingConnection(params) # Connect to CloudAMQP
        channel = connection.channel() # start a channel
        routing_key = queue_name
    
        message = str(results)

        if not message:
            message = "No results for id"

        message = message.encode()

        channel.confirm_delivery()
     
        channel.exchange_declare(exchange=queue_name, exchange_type='direct')
        result = channel.queue_declare(queue_name, durable=True, exclusive=False)

        binding_key = queue_name

        channel.queue_bind(exchange=queue_name, queue=queue_name, routing_key=binding_key)
        
        channel.basic_publish(exchange=queue_name, 
        routing_key=routing_key, body=message, 
        properties=pika.BasicProperties(content_type = "text/plain"), mandatory=True) # False: dont care whether someone picks up message  

    except pika.exceptions.UnroutableError:
        print("Error in send response. Unroutable")
    except:
        print("Caught an exception")

    finally:
        channel.close()
        connection.close()