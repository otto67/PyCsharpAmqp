
import pika, os, logging, json, threading, time

logging.basicConfig()

def sendMsg(cmd, entry):
    url = os.getenv('CLOUDAMQP_URL')
    if not url:
        print("Environment variable CLOUDAMQP_URL not set")
        exit(-1)

    params = pika.URLParameters(url)
    params.socket_timeout = 5
    connection = pika.BlockingConnection(params) # Connect to CloudAMQP
    channel = connection.channel() # start a channel
    routing_key = 'post.mydatabase.test'

    msg = {'cmd': cmd, 'data':entry}
    message = json.dumps(msg)
    channel.confirm_delivery()

    try:
        channel.basic_publish(
        exchange='db_writer', routing_key=routing_key, body=message, mandatory=True)
        print ("[x] Message sent to consumer")
    except pika.exceptions.UnroutableError:
        print("Message could not be delivered. No consumer.")   

    finally:
        channel.close()
        connection.close()

def insertEntry(): 
    entry = ['Harry', 'Doe', 'harry@doe.com']
    cmd = 'add'
    sendMsg(cmd, entry)

def get_response(ch, method, properties, body):
    message = json.loads(body.decode())
    print("Get response, got: " + message)
    exit(0)

def createQueueAndWait(queue_name):
    url = os.getenv('CLOUDAMQP_URL')
    if not url:
        print("Environment variable CLOUDAMQP_URL not set")
        exit(-1)    
    
    params = pika.URLParameters(url)
    connection = pika.BlockingConnection(params)
    channel = connection.channel() # start a channel
    channel.exchange_declare(exchange=queue_name, exchange_type='direct')
    result = channel.queue_declare(queue_name, exclusive=False)

    binding_key = queue_name

    channel.queue_bind(exchange=queue_name, queue=queue_name, routing_key=binding_key)
    print(' Waiting for response to database query')
    channel.basic_consume(queue=queue_name, on_message_callback=get_response, auto_ack=True)
    channel.start_consuming()

def listEntries(): 
    cmd = 'list'
    entry = ['myqueue']

    print("listentries : Before starting thread ")

    mythread = threading.Thread(target=createQueueAndWait, args=('myqueue',))
    mythread.daemon = True
    mythread.start()
    
    time.sleep(1)

    print("listentries : Sending list message ")
    sendMsg(cmd, entry)

    # Delete the queue and clean up later


print("Adding entry")
insertEntry()

print("Listing entries")
listEntries()
