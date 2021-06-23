
import socket                                                       # импорт нужных модулей
import time
import os
import threading

s = socket.socket()                                                 # создаем обьект сокета
s.connect(('127.0.0.1', 4040))                                      # подключаемся к серваку

conf_file_path = 'conf_f.txt'                                    # расположение конфиг файла
#4AA95827-0199-4F82-9CC5-0FB36F1056D9
full_message_by_byte = bytes('', 'utf-8')                                                # переменная
message = ""
id = "python-000000901"                                ##################################################################
light = 0                                                             # флаг отвечающий за вкл\выкл свет
main_event = True                                                   # флаг работы датчика

time_delay = 0                                                          # промежутки между отправкой сообщения
last_time = time.time()                                             # фиксируем время начала

fake_time = 0#X-X#                                                  # "#X-X#" - все что помечено этим сиволом относится к формальной симуляции времени/показателей света
fake_out_light = 0#X-X# 

sun_day_start = 0
sun_day_end = 0

get_sett = False

#execution_queue_arr                                                # очередь сообщений которые отослал сервер

def set_settings():                                                 #метод установки настроек из файла

    global conf_file_path
    global id
    global time_delay
    global sun_day_start
    global sun_day_end
    global light

    if os.path.exists(conf_file_path):
        lines = []
        with open(conf_file_path) as f:
            lines = f.readlines()
        count = 0
        for line in lines:
            if count == 0: conf_file_path = line.replace("\n","")
            elif count == 1: id = line.replace("\n","")
            elif count == 2: time_delay = int(line.replace("\n",""))
            elif count == 3: sun_day_start = int(line.replace("\n",""))
            elif count == 4: sun_day_end =  int(line.replace("\n",""))
            elif count == 5: light = int(line.replace("\n",""))
            count += 1 
    else:
        print("asdas")
        my_file = open(conf_file_path, "w+")
        my_file.close()
        set_settings()

def save_settings():                                                # метод сохранения настроек
    global conf_file_path
    global id
    global time_delay
    global sun_day_start
    global sun_day_end
    global light
    f = open(conf_file_path, 'w')
    L = [conf_file_path+"\n", id+"\n", str(time_delay)+"\n", str(sun_day_start)+"\n", str(sun_day_end)+"\n",str(light)]
    f.writelines(L)  
    f.close()

def get_controller_info():                                          # подразумевается что день идет 48 секунд
    global fake_out_light
    global fake_time

    if fake_time <= 16: 
        fake_out_light += 326
    elif fake_time > 32:
        fake_out_light -= 326
    if fake_time == 48:
        fake_time = 0

def light_chek():
    global fake_out_light
    global fake_time
    global light
    if (fake_out_light < 5200 and (fake_time > 6 or fake_time <= 32)):
        light = 2
    elif (fake_out_light > 5200): light = 1

def timer(last_time):                                               # метод позволяющий узнать сколько прошло времени с последней отправки сообщения
    return time.time() - last_time

def final_method_sending():                                   
    global time_delay
    global last_time
    if timer(last_time) >= time_delay:                              # после отправки сообщения сбрасываем таймер
        send_message()
        print('message done' + str(timer(last_time)))
        last_time = time.time()
    
def send_message():                                                 # метод отправки сообщения
    s.sendall(construct(id,message,fake_time))

def send_message_get_sett():
    global id
    s.sendall(str.encode(id+'|'+"get_settings|"))
def server_message_listener():                                      # метод чтения сообщений от сервера
    global sun_day_start
    global sun_day_end
    global get_id
    global id
    while True:
        print("ждем сообщение")
        #execution_queue_arr.append(s.recv(1024).decode("utf-8"))
        quest = s.recv(1024).decode("utf-8")
        if not get_sett:
            settt =  quest.split('|')
            sun_day_start = sett[0]
            sunsun_day_end = sett[1]
            save_settings()
            get_sett = True
        if get_sett:
            quest = quest.replace("|","").replace("-","")
            sun_day_start = int(quest)/100
            sun_day_end = int(quest)%100
        time.sleep(5)

def construct(id, message, fake_time):                                         # метод преобразования id и параметров датчика в строку, затем в байты
    return str.encode(id +'|'+"send_data"+'|'+ str(fake_out_light)+'|'+ str(fake_time))

def main():
    server_message_listener_thread = threading.Thread(target=server_message_listener) # создаем поток для приема сообщений
    server_message_listener_thread.start()                                            # запускаем его
    global fake_time#X-X#
    global execution_queue_arr

    while (get_sett == False):
        print("запрос на настройки")
        send_message_get_sett()
        time.sleep(1)
    while main_event:                                               # если main_event == False, заканчиваем цикл отправки сообщений и закрываем сокет
        if get_sett:
            final_method_sending()
            get_controller_info()
            time.sleep(1)
            fake_time += 1#X-X#

set_settings()
main()

s.close() 



