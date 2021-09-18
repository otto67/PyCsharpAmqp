import pika, json
import os, sys
import threading
import time
import sqlite3
import response as sender

recv_count = 0
mutex = threading.Lock()

url = os.getenv('CLOUDAMQP_URL')
if not url:
    print("Environment variable CLOUDAMQP_URL not set")
    sys.exit(-1)

def createThread(id, message):
    mythr = threading.Thread(target=runThread, args=(id,message))
    mythr.daemon = True
    mythr.start()
    

def queue_callback(ch, method, properties, body):
    global recv_count
    recv_count += 1
    message = body.decode()
    createThread(recv_count, message)

def createTable():
    conn = sqlite3.connect('taskdb.db')
    c = conn.cursor()
    c.execute("""CREATE TABLE IF NOT EXISTS tasks (
        id integer PRIMARY KEY,
        description text NOT NULL,
        completed integer        
    )""")

    conn.commit()
    conn.close()

def addEntry(mylist):
    # Assume database file is in running directory 
    retval = 0
    if not os.path.isfile('taskdb.db'):
        return -2

    conn = sqlite3.connect('taskdb.db')
    c = conn.cursor()

    id = mylist[0]
    desc = mylist[1]

    try:
        c.execute("INSERT INTO tasks VALUES (?, ?, ?)", (id, desc, 0))
        conn.commit()
        conn.close()
        print("Added element with id " + id + " and description " + desc)
                
    except sqlite3.IntegrityError:
        print("Unsuccessful add. Id " + str(id) + " already exists")
        conn.close()
        retval = -1
    
    finally:
        return retval 
    


def printAll(queue_name):
    
    # Assume database file is in running directory
    if not os.path.isfile('taskdb.db'):
        return 

    conn = sqlite3.connect('taskdb.db')
    if not conn:
        return 

    c = conn.cursor()
    c.execute("SELECT * FROM tasks")
    tasks = c.fetchall()
    conn.close()

    mystring = ""
    for task in tasks:
        for j in range(len(task)):
            mystring += str(task[j])
            if (j < len(task) - 1):
                mystring += ';'
        mystring +=':'

    mystring = mystring[:-1]

    print("Printall: " + mystring)

    sender.sendResponse(mystring, queue_name)
    

def findEntry(id, queue_name):
    # Assume database file is in running directory
    if not os.path.isfile('taskdb.db'):
        return 

    conn = sqlite3.connect('taskdb.db')
    if not conn:
        return 

    c = conn.cursor()
    c.execute('SELECT * FROM tasks WHERE id=?', (id,))
    tasks = c.fetchall()
    conn.close()

    mystring = ""
    for task in tasks:
        for j in range(len(task)):
            mystring += str(task[j])
            if (j < len(task)-1):
                mystring += ';'
        mystring +=':'

    mystring = mystring[:-1]

    print("FindEntry: ", mystring)
    sender.sendResponse(mystring, queue_name)

def removeEntry(id):
    # Assume database file is in running directory
    
    if not os.path.isfile('taskdb.db'):
        return 

    conn = sqlite3.connect('taskdb.db')
    if not conn:
        return 

    c = conn.cursor()
    try:
        c.execute('DELETE FROM tasks WHERE id=?', (id,))
        retval = c.rowcount
        conn.commit()
        conn.close()
        print("Removed " + retval + " rows from table") 
    except:
        conn.close()
        
    finally:
        pass

def completeEntry(id):
    # Assume database file is in running directory
    
    if not os.path.isfile('taskdb.db'):
        return

    conn = sqlite3.connect('taskdb.db')
    if not conn:
        return

    c = conn.cursor()   
   
    try:
        c.execute('UPDATE tasks SET completed=? WHERE id=?', (1, id))

        print("After execute")

        retval = c.rowcount
        conn.commit()
        
    except:
        e = sys.exc_info()[0]        
        print("Exception from completeEntry", e)
    finally:
        conn.close() 

def runThread(id, message):

    retval = 0
 
 #    while not mutex.acquire(blocking=False):
 #       print(f"Thread id {id}: Mutex locked")
 #       time.sleep(0.5)

    mutex.acquire()
 
    mylist = message.split(';')
    if mylist[0] == 'Add':
        addEntry(mylist[1:])
    elif mylist[0] == 'Delete':
        removeEntry(mylist[1])
    elif mylist[0] == 'Complete':
         completeEntry(mylist[1])
    else: # List request
        queue_name = 'task_response'
        if mylist[1].lower() == 'all':
            printAll(queue_name)
        else:
            findEntry(mylist[1], queue_name)
    mutex.release()
    

# Creating a database table named tasks
createTable()

params = pika.URLParameters(url)
connection = pika.BlockingConnection(params)
channel = connection.channel() # start a channel
channel.exchange_declare(exchange='task_exch', exchange_type='topic')
result = channel.queue_declare('task_queue', durable=True) # , exclusive=True)
queue_name = result.method.queue
binding_key = 'post.*.test'

channel.queue_bind(exchange='task_exch', queue=queue_name, routing_key=binding_key)
print(' [*] Waiting for database queries. To exit press CTRL+C')

try: 
    channel.basic_consume(queue=queue_name, on_message_callback=queue_callback, auto_ack=True)
    channel.start_consuming()

except KeyboardInterrupt:
    print("Keyboard interrupt! Exiting, bye")
    sys.exit(0)
